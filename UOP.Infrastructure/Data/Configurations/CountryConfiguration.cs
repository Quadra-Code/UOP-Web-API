using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UOP.Domain.Entities;

namespace UOP.Infrastructure.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> entity)
        {
            entity.ToTable("Country", "Core");
            entity.HasKey(e => e.CountryId).HasName("PK_Core.Country");
            entity.Property(e => e.CountryId)
                .ValueGeneratedNever()
                .HasColumnName("CountryID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime2");
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            entity.Property(e => e.FlagImage).HasMaxLength(500);
            entity.Property(e => e.FlagThumbnail).HasMaxLength(500);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(50);
            entity.Property(e => e.Order).HasMaxLength(10);
            entity.Property(e => e.Order).HasColumnName("Order");
            entity.HasIndex(e => e.Order);

            entity.HasMany(e => e.States)
                .WithOne(c => c.Country)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_State_Country");
        }
    }
}
