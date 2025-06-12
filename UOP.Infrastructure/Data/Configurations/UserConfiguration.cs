using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UOP.Domain.Entities;

namespace UOP.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("User", "UserManagement");
            entity.HasKey(e => e.UserId).HasName("PK_UserManagement.User");
            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime2");
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.UserName).HasMaxLength(200);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(100);
            entity.Property(e => e.PictureUrl).HasMaxLength(500);
            entity.Property(e => e.CoverPhotoUrl).HasMaxLength(500);
            entity.Property(e => e.UserPictureUrl).HasMaxLength(500);

            entity.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_UserRole_User");

            entity.HasMany(u => u.PhoneNumbers)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PhoneNumber_User");
        }
    }
}
