using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UOP.Domain.Entities;

namespace UOP.Infrastructure.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> entity)
        {
            entity.ToTable("Address", "UserManagement");
            entity.HasKey(e => e.AddressId).HasName("PK_UserManagement.Address");
            entity.Property(e => e.AddressId)
                .ValueGeneratedNever()
                .HasColumnName("AddressID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime2");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            entity.Property(e => e.AddressLine1).HasMaxLength(300);
            entity.Property(e => e.AddressLine2).HasMaxLength(400);
            entity.Property(e => e.PostalCode).HasMaxLength(15);
            entity.Property(e => e.AppartmentNumber).HasMaxLength(5);
            entity.Property(e => e.FloorNumber).HasMaxLength(3);

            entity.HasOne<User>()
                .WithMany(c => c.Addresses)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Address_User");

            entity.HasOne<City>()
                .WithMany(c => c.Addresses)
                .HasForeignKey(c => c.CityId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Address_City");
        }
    }
}
