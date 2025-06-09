using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UOP.Domain.Entities;

namespace UOP.Infrastructure.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> entity)
        {
            entity.ToTable("Permission", "PermissionManagment");

            entity.Property(e => e.PermissionId)
                .ValueGeneratedNever()
                .HasColumnName("PermissionID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime2");
            entity.Property(e => e.Name).HasMaxLength(100);
            //entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.PermissionTypeId).HasColumnName("PermissionTypeID");
            //entity.Property(e => e.ApplicationEntityId).HasColumnName("ApplicationEntityID");
            entity.Property(e => e.Order).HasColumnName("Order");
            entity.HasIndex(e => e.Order);
            entity.HasOne(d => d.PermissionType).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.PermissionTypeId)
                .HasConstraintName("FK_Permission_PermissionType");

            //entity.HasOne(d => d.ApplicationEntity).WithMany(p => p.Permissions)
            //        .HasForeignKey(d => d.ApplicationEntityId)
            //        .HasConstraintName("FK_Permission_ApplicationEntity")
            //        .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
