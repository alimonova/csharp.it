using System;
using csharp.it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp.it.Configurations
{
	public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.ToTable("UserGroups");
            builder.HasKey(ug => ug.Id);

            builder.Property(ug => ug.Progress).IsRequired();

            builder
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.UserId);

            builder
                .HasOne(ug => ug.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(ug => ug.GroupId);
        }
    }
}

