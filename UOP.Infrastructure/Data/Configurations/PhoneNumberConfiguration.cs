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
    internal class PhoneNumberConfiguration
    {
        public void Configure(EntityTypeBuilder<PhoneNumber> entity)
        {
            entity.ToTable("PhoneNumber", "UserManagement");
            entity.HasKey(e => e.PhoneNumberId).HasName("PK_Core.PhoneNumber");
            entity.Property(e => e.PhoneNumberId)
                .ValueGeneratedNever()
                .HasColumnName("PhoneNumberID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime2");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            entity.Property(e => e.Number).HasMaxLength(15);
            entity.Property(e => e.Code).HasMaxLength(5);

            entity.HasOne(ur => ur.User)
                .WithMany(u => u.PhoneNumbers)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PhoneNumber_User");
        }
    }
}
