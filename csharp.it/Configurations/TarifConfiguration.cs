using System;
using csharp.it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp.it.Configurations
{
	public class TarifConfiguration : IEntityTypeConfiguration<Tarif>
    {
        public void Configure(EntityTypeBuilder<Tarif> builder)
        {
            builder.ToTable("Tarifs");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Price).IsRequired();
            builder.Property(t => t.Currency).IsRequired();
            builder.Property(t => t.Description).IsRequired();
            builder.Property(t => t.Access).IsRequired();

            builder
                .HasOne(t => t.Course)
                .WithMany(c => c.Tarifs)
                .HasForeignKey(t => t.CourseId);
        }
    }
}

