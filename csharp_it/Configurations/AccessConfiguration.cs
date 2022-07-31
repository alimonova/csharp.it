using System;
using csharp_it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp_it.Configurations
{
	public class AccessConfiguration : IEntityTypeConfiguration<Access>
    {
        public void Configure(EntityTypeBuilder<Access> builder)
        {
            builder.ToTable("Accesses");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name).IsRequired();

            builder
                .HasMany(a => a.TarifAccesses)
                .WithOne(ta => ta.Access)
                .HasForeignKey(ta => ta.AccessId)
                .OnDelete(DeleteBehavior.NoAction);
        }
	}
}

