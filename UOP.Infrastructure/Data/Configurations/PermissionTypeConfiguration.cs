using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UOP.Domain.Entities;

namespace UOP.Infrastructure.Data.Configurations
{
    //public class PermissionTypeConfiguration : IEntityTypeConfiguration<PermissionType>
    //{
    //    public void Configure(EntityTypeBuilder<PermissionType> entity)
    //    {
    //        entity.ToTable("PermissionType", "PermissionManagment");

    //        entity.HasKey(e => e.PermissionTypeId).HasName("PK_PermissionManagment.PermissionType");

    //        entity.Property(e => e.PermissionTypeId)
    //            .ValueGeneratedNever()
    //            .HasColumnName("PermissionTypeID");
    //        entity.Property(e => e.CreatedDate).HasColumnType("datetime2");
    //        entity.Property(e => e.UpdatedDate).HasColumnType("datetime2");
    //        entity.Property(e => e.Name).HasMaxLength(100);
    //        entity.Property(e => e.CreatedBy).HasMaxLength(100);
    //        entity.Property(e => e.UpdatedBy).HasMaxLength(100);
    //        //entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

    //    }
    //}
}
