using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.model
{
    public class KraDbContext : DbContext
    {
        public DbSet<Facility> Facilities { get; set; }
        
        public DbSet<Visit> Visits { get; set; }

        public DbSet<Household> Households { get; set; }

        public KraDbContext(DbContextOptions options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Facility>()
                .HasIndex(p => new { p.Name })
                .IsUnique();

            modelBuilder.Entity<Visit>()
                .HasIndex(p => new { p.From, p.To, p.IsCanceled})
                .IsUnique();
        }
    }
}
