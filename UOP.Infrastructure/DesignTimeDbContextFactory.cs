﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using UOP.Infrastructure.Data;
using Microsoft.Extensions.Configuration;

namespace UOP.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"))
                .Build();
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            //var connectionString = configuration.GetConnectionString("LocalConnection");
            var connectionString = configuration.GetConnectionString("RemoteConnection");
            //var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new AppDbContext(builder.Options);
        }
    }
}
