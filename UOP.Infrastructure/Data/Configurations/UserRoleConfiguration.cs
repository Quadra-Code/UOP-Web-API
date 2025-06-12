using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UOP.Domain.Entities;

namespace UOP.Infrastructure.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> entity)
        {
            entity.ToTable("UserRole", "Core");
            entity.HasKey(e => e.UserRoleId).HasName("PK_UserManagement.UserRole");
            entity.Property(e => e.UserRoleId)
                .ValueGeneratedNever()
                .HasColumnName("UserRoleID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime2");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy).HasMaxLength(100);

            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_UserRole_User");

            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.RoleUsers)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_UserRole_Role");
        }
    }
}
