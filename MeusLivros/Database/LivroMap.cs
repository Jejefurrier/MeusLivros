using MeusLivros.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeusLivros.Database
{
    public class LivroMap : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.ToTable("Livro");

            builder.HasKey(c => c.ID);

            builder.Property(c => c.IDDono)
                .IsRequired()
                .HasColumnName("IDDono")
                .IsUnicode();


            builder.Property(c => c.Autor)
                .IsRequired()
                .HasColumnName("Autor");

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnName("Nome");

            builder.Property(c => c.Link)
                .IsRequired()
                .HasColumnName("Link");
        }
    }
}
