using Microsoft.EntityFrameworkCore;
using KertKerdes.Models;
using System.Collections.Generic;

namespace KertKerdes.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Kerdes> Kerdesek { get; set; }
        public DbSet<Valasz> Valaszok { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Valasz>()
                .HasOne(v => v.Kerdes)
                .WithMany(k => k.Valaszok)
                .HasForeignKey(v => v.KerdesId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}