using System;
using csharp_it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp_it.Configurations
{
	public class TarifAccessConfiguration : IEntityTypeConfiguration<TarifAccess>
    {
        public void Configure(EntityTypeBuilder<TarifAccess> builder)
        {
            builder.ToTable("TarifAccesses");
            builder.HasKey(ta => ta.Id);

            builder
                .HasOne(ta => ta.Tarif)
                .WithMany(t => t.TarifAccesses)
                .HasForeignKey(ta => ta.TarifId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(ta => ta.Access)
                .WithMany(a => a.TarifAccesses)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(ta => ta.AccessId);
        }
    }
}

