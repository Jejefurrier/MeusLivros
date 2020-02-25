using MeusLivros.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeusLivros.Database
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Username)
                .IsRequired()
                .HasColumnName("Username")
                .IsUnicode();
                

            builder.Property(c => c.Password)
                .IsRequired()
                .HasColumnName("Password");

            builder.Property(c => c.DataIncricao)
                .IsRequired()
                .HasColumnName("DataIncricao");

            builder.Property(c => c.Role)
                .IsRequired()
                .HasColumnName("Role");
        }
    }
}
