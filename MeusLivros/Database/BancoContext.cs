using MeusLivros.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeusLivros.Database
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Livro> Livros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(new UserMap().Configure);
            modelBuilder.Entity<Livro>(new LivroMap().Configure);
        }

    }
}
