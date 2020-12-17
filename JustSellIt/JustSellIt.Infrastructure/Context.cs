using JustSellIt.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustSellIt.Infrastructure
{
    public class Context : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<OwnerContact> OwnersContact { get; set; }
        public DbSet<ProductStatus> ProductStatuses { get; set; }
        public DbSet<Sex> Sex { get; set; }

        public Context(DbContextOptions options) : base(options)
        {
        }

        // Fluent API
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // relations one to one 
            builder.Entity<Owner>()
                .HasOne(a => a.OwnerContact).WithOne(b => b.Owner)
                .HasForeignKey<OwnerContact>(e => e.OwnerRef);
        }
    }
}
