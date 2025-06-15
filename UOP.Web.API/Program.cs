
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

namespace UOP.Web.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
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

            // Configure Mapster
            MapsterConfig.Configure();

            builder.Services.AddControllers(options =>
            {
                options.Conventions.Add(new DefaultProducesResponseTypeConvention());
            });

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

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Here i used to use the following configuration in .net 8 and before to add authentication and authorization in swagger interface, so how can i do the same in .net 9 and above for both swagger and scalar ?
            //builder.Services.AddSwaggerGen(sa =>
            //{
            //    sa.SwaggerDoc("v1", new OpenApiInfo //here the name (v1) must be (v1) small ant there is no relation with the version below
            //    {
            //        Title = "Client API",
            //        Version = "v1"
            //    });
            //    sa.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        In = ParameterLocation.Header,
            //        Description = "Please Insert JWT with Bearer Into Field",
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.ApiKey
            //    });
            //    sa.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Id = "Bearer",
            //                    Type = ReferenceType.SecurityScheme
            //                }
            //            },
            //            new string[] { }
            //        }
            //    });
            //});

            // For Scalar configuration
            //builder.Services.AddScalar(options =>
            //{
            //    options.SpecUrl = "/swagger/v1/swagger.json";
            //    options.Config = new ScalarConfig
            //    {
            //        Authentication = new ScalarAuthentication
            //        {
            //            Default = new ScalarAuthenticationDefault
            //            {
            //                Type = "http",
            //                Scheme = "bearer"
            //            }
            //        }
            //    };
            //});

            //builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(setup =>
            {
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
                setup.AddSecurityDefinition("Tenant", new OpenApiSecurityScheme
                {
                    Name = "Tenant",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Enter the Tenant header value",
                    Scheme = "Tenant"
                });
                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Tenant"
                            },
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                // Tell Swagger to Use XML Comments
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //setup.IncludeXmlComments(xmlPath);


            });

            //builder.Services.AddOpenApi(options =>
            //{
            //    options.UseApiEndpoint("/openapi/v1.json");

            //    // Add JWT Bearer security definition
            //    options.AddSecurityScheme("Bearer", new()
            //    {
            //        Type = "http",
            //        Scheme = "bearer",
            //        BearerFormat = "JWT",
            //        Description = "JWT Authorization header using the Bearer scheme."
            //    });

            //    options.OperationFilter<AuthResponsesOperationFilter>();
            //});

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
                //envConnectString = builder.Configuration.GetConnectionString("RemoteConnection");
                envConnectString = builder.Configuration.GetConnectionString("DefaultConnection");
            }
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(envConnectString).EnableDetailedErrors(true), ServiceLifetime.Scoped);


            //builder.Services.AddControllers(options =>
            //{
            //    options.Filters.Add<AuthorizeFilter>();
            //});
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.MapOpenApi();
                app.MapScalarApiReference();
                app.UseSwagger();
                app.UseSwaggerUI(options => options.SwaggerEndpoint(url: "/openapi/v1.json", name: "V1"));
            //}
            app.UseHttpsRedirection();
            app.UseCors("Default");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            
            //app.UseScalarApiReference(options =>
            //{
            //    options.SpecUrl = "/openapi/v1.json";
            //    options.ConfigObject = new ScalarConfigObject
            //    {
            //        Authentication = new ScalarAuthenticationConfig
            //        {
            //            Default = new ScalarAuthenticationDefaultConfig
            //            {
            //                Type = "http",
            //                Scheme = "bearer"
            //            }
            //        }
            //    };
            //});
            app.UseResponseCaching();
            app.MapControllers();

            app.Run();
        }
    }
}
