
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using UOP.Application.Common.Interfaces;
using UOP.Application.Common.Mappers;
using UOP.Application.Common.Services;
using UOP.Domain.Entities;
using UOP.Domain.Interfaces;
using UOP.Infrastructure.Data;
using UOP.Infrastructure.Repositories;
using UOP.Web.API.Attributes;
using Microsoft.OpenApi.Models;
using UOP.Application.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace UOP.Web.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            // Configure Mapster
            MapsterConfig.Configure();

            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireConfirmedEmail");
                options.Password.RequireDigit = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireDigit");
                options.Password.RequiredLength = builder.Configuration.GetValue<int>("PasswordRequirements:MinimumLength");
                options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireSpecialCharacter");
                options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireUppercase");
                options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireLowercase");
                options.User.RequireUniqueEmail = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireUniqueEmail");
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            //builder.Services.AddScoped(typeof(IPermissionService), typeof(PermissionService));

            builder.Services.AddScoped(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));
            builder.Services.AddScoped(typeof(ILookupService<>), typeof(LookupService<>));
            builder.Services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = 64 * 1024 * 1024; // 64MB
                options.UseCaseSensitivePaths = false;
            });
            var envConnectString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_DefaultConnection");
            if (string.IsNullOrEmpty(envConnectString))
            {
                //envConnectString = builder.Configuration.GetConnectionString("LocalConnection");
                envConnectString = builder.Configuration.GetConnectionString("RemoteConnection");
                //envConnectString = builder.Configuration.GetConnectionString("DefaultConnection");
            }
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(envConnectString).EnableDetailedErrors(true), ServiceLifetime.Scoped);

            // Add OpenAPI and Swagger configurations
            builder.Services.AddSwaggerGen(sa =>
            {
                sa.SwaggerDoc("v1", new OpenApiInfo //here the name (v1) must be (v1) small and there is no relation with the version below
                {
                    Title = "Upping Online Pharma API",
                    Version = "v1"
                });
                sa.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please Insert JWT with Bearer Into Field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                sa.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new string[] { }
                    }
                });
            });

            // Add Authentication and Authorization
            builder.Services.AddAuthentication(op =>
            {
                op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op =>
            {
                op.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["jwt:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"]))
                };
            });

            // Add CORS policies
            builder.Services.AddCors(op =>
            {
                op.AddPolicy("Default", policy =>
                {
                    //policy.WithOrigins("http://localhost:4200", "http://anydomain:domainport", "null")
                    //.WithHeaders("Authorization")
                    //.WithMethods("Post", "Get");
                    policy.AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod();
                });
                op.AddPolicy("Production", policy =>
                {
                    policy.WithOrigins("https://allbirds-git-elghoul-mahmoud-elsonbatys-projects.vercel.app", "https://allbirds-orcin.vercel.app", "http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            // Add Controllers
            builder.Services.AddControllers(options =>
            {
                options.Conventions.Add(new DefaultProducesResponseTypeConvention());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.MapOpenApi();
                //app.MapGet("/openapi/v1.json", () => app.Services.GetRequiredService<ISwaggerProvider>().GetSwagger("v1"));
                //app.MapGet("/openapi/v1.json", () => Results.Json(app.Services.GetRequiredService<ISwaggerProvider>().GetSwagger("v1")) );
                app.MapGet("/openapi/v1.json", () => Results.Redirect("/swagger/v1/swagger.json") );
                app.MapScalarApiReference();
                
            //    app.UseSwagger();
            //app.UseSwaggerUI(options => options.SwaggerEndpoint(url: "/openapi/v1.json", name: "V1"));
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.RoutePrefix = string.Empty;  // Optional: Use empty string to serve Swagger at the root
                });

            //}

            app.UseHttpsRedirection();
            app.UseCors("Default");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseResponseCaching();
            app.MapControllers();

            app.Run();
        }
    }
}
