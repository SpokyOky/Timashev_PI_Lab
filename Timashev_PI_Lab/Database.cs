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

            modelBuilder.Entity<RecipeTechCard>().HasKey(rtc => new { rtc.RId, rtc.TCId });

            modelBuilder.Entity<RecipeTechCard>().HasOne(rtc => rtc.Recipe)
                .WithMany(r => r.RecipeTechCards)
                .HasForeignKey(rtc => rtc.RId);
            modelBuilder.Entity<RecipeTechCard>().HasOne(rtc => rtc.TechCard)
                .WithMany(tc => tc.RecipeTechCards)
                .HasForeignKey(rtc => rtc.TCId);
        }

        public virtual DbSet<User> Users { set; get; }
        public virtual DbSet<Product> Products { set; get; }
        public virtual DbSet<ChemElement> ChemElements { set; get; }
        public virtual DbSet<Recipe> Recipes { set; get; }
        public virtual DbSet<ProductChemElement> ProductChemElements { set; get; }
        public virtual DbSet<TechCard> TechCards{ set; get; }
        public virtual DbSet<RecipeTechCard> RecipeTechCards { set; get; }
        public virtual DbSet<ProductRecipe> ProductRecipes { set; get; }
    }
}
