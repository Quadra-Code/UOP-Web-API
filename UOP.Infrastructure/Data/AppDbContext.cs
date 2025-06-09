using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Entities;

namespace UOP.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Permission> Permissions { get; set; }

        public virtual DbSet<PermissionType> PermissionTypes { get; set; }

        public virtual DbSet<Category> Products { get; set; }

    }
}
