using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UOP.Domain.Entities;

namespace UOP.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> entity)
        {
            entity.ToTable("Role", "UserManagement");
            entity.HasKey(e => e.RoleId).HasName("PK_UserManagement.Role");
            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime2");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NormalizedName).HasMaxLength(100);

            entity.HasMany(r => r.RoleUsers)
                .WithOne(ru => ru.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_UserRole_Role");
        }
    }
}
