using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Entities;

namespace UOP.Infrastructure.Data.Configurations
{
    public class Productconfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.ToTable("Product", "Core");
            entity.HasKey(e => e.ProductId).HasName("PK_Core.Product");
            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("ProductID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime2");
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En).HasMaxLength(100);
            entity.Property(e => e.Order).HasColumnName("Order");
            entity.HasIndex(e => e.Order);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Product_Category");

        }
    }
}
