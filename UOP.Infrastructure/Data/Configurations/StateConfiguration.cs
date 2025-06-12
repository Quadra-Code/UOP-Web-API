using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UOP.Domain.Entities;

namespace UOP.Infrastructure.Data.Configurations
{
    public class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> entity)
        {
            entity.ToTable("State", "Core");
            entity.HasKey(e => e.StateId).HasName("PK_Core.State");
            entity.Property(e => e.StateId)
                .ValueGeneratedNever()
                .HasColumnName("StateID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime2");
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            entity.Property(e => e.Order).HasMaxLength(10);
            entity.Property(e => e.MapUrl).HasMaxLength(500);
            entity.Property(e => e.Order).HasColumnName("Order");
            entity.HasIndex(e => e.Order);

            entity.HasOne(c => c.Country)
                .WithMany(c => c.States)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_State_Country");

            entity.HasMany(e => e.Cities)
                .WithOne(c => c.State)
                .HasForeignKey(c => c.StateId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_City_State");
        }
    }
}
