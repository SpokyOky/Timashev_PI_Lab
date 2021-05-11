using System;
using System.Linq;
using Timashev_PI_Lab.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Timashev_PI_Lab
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options)
                   : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductChemElement>().HasKey(rce => new { rce.PId, rce.CEId });

            modelBuilder.Entity<ProductChemElement>().HasOne(rce => rce.Product)
                .WithMany(r => r.ProductChemElements)
                .HasForeignKey(rce => rce.PId);
            modelBuilder.Entity<ProductChemElement>().HasOne(rce => rce.ChemElement)
                .WithMany(ce => ce.ProductChemElements)
                .HasForeignKey(rce => rce.CEId);

            modelBuilder.Entity<ProductRecipe>().HasKey(pr => new { pr.PId, pr.RId });

            modelBuilder.Entity<ProductRecipe>().HasOne(pr => pr.Product)
                .WithMany(p => p.ProductRecipes)
                .HasForeignKey(pr => pr.PId);
            modelBuilder.Entity<ProductRecipe>().HasOne(pr => pr.Recipe)
                .WithMany(r => r.ProductRecipes)
                .HasForeignKey(pr => pr.RId);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 0,
                    FIO = "admin",
                    Login = "admin",
                    Password = "admin"
                });
            modelBuilder.Entity<ChemElement>().HasData(
                new ChemElement[]
                {
                    new ChemElement{ Id = 0, Name = "Б" },
                    new ChemElement{ Id = 1, Name = "Ж" },
                    new ChemElement{ Id = 2, Name = "У" },
                    new ChemElement{ Id = 3, Name = "B1" },
                    new ChemElement{ Id = 4, Name = "C" },
                    new ChemElement{ Id = 5, Name = "A" },
                    new ChemElement{ Id = 6, Name = "E" },
                    new ChemElement{ Id = 7, Name = "Ca" },
                    new ChemElement{ Id = 8, Name = "P" },
                    new ChemElement{ Id = 9, Name = "Mg" }
                });
        }

        public virtual DbSet<User> Users { set; get; }
        public virtual DbSet<Product> Products { set; get; }
        public virtual DbSet<ChemElement> ChemElements { set; get; }
        public virtual DbSet<Recipe> Recipes { set; get; }
        public virtual DbSet<ProductChemElement> ProductChemElements { set; get; }
        public virtual DbSet<TechCard> TechCards{ set; get; }
        public virtual DbSet<ProductRecipe> ProductRecipes { set; get; }
    }
}
