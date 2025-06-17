using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UOP.Domain.Entities;

namespace UOP.Infrastructure.Data.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> entity)
        {
            entity.ToTable("City", "Core");
            entity.HasKey(e => e.CityId).HasName("PK_Core.City");
            entity.Property(e => e.CityId)
                .ValueGeneratedNever()
                .HasColumnName("CityID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime2");
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            entity.Property(e => e.Order).HasMaxLength(10);
            entity.Property(e => e.Order).HasColumnName("Order");
            entity.HasIndex(e => e.Order);

            entity.HasOne<State>()
                .WithMany(c => c.Cities)
                .HasForeignKey(c => c.StateId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_City_State");

            entity.HasMany(c => c.Addresses)
                .WithOne(c => c.City)
                .HasForeignKey(c => c.CityId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Address_City");
        }
    }
}
