using System;
using System.Linq;
using Timashev_PI_Lab.Models;
using Microsoft.EntityFrameworkCore;

namespace Timashev_PI_Lab
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options)
                   : base(options)
        {
            //SampleData.Initialize(this);
            // Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductChemElement>().HasKey(of => new { of.PId, of.CEId });

            modelBuilder.Entity<ProductChemElement>().HasOne(of => of.Product)
                .WithMany(o => o.ProductChemElements)
                .HasForeignKey(of => of.PId);
            modelBuilder.Entity<ProductChemElement>().HasOne(of => of.ChemElement)
                .WithMany(o => o.ProductChemElements)
                .HasForeignKey(of => of.CEId);

            modelBuilder.Entity<ProductRecipe>().HasKey(fm => new { fm.PId, fm.RId });

            modelBuilder.Entity<ProductRecipe>().HasOne(fm => fm.Product)
                .WithMany(f => f.ProductRecipes)
                .HasForeignKey(fm => fm.PId);
            modelBuilder.Entity<ProductRecipe>().HasOne(fm => fm.Recipe)
                .WithMany(f => f.ProductRecipes)
                .HasForeignKey(fm => fm.RId);
        }

        public virtual DbSet<User> Users { set; get; }
        public virtual DbSet<Product> Products { set; get; }
        public virtual DbSet<ChemElement> ChemElements { set; get; }
        public virtual DbSet<Recipe> Recipes { set; get; }
        public virtual DbSet<ProductChemElement> ProductChemElements { set; get; }
        public virtual DbSet<ProductRecipe> ProductRecipes { set; get; }
    }
}
