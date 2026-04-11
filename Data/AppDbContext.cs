using KertKerdes.Models;
using Microsoft.EntityFrameworkCore;

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

        public DbSet<Felhasznalo> Felhasznalok { get; set; }

        public DbSet<Temakor> Temakorok { get; set; }

        public DbSet<Cimke> Cimkek { get; set; }

        public DbSet<KerdesCimke> KerdesCimkek { get; set; }

        public DbSet<Szavazat> Szavazatok { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kerdes>()
                .HasOne(k => k.Felhasznalo)
                .WithMany(f => f.Kerdesek)
                .HasForeignKey(k => k.FelhasznaloId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Kerdes>()
                .HasOne(k => k.Temakor)
                .WithMany()
                .HasForeignKey(k => k.TemakorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Valasz>()
                .HasOne(v => v.Felhasznalo)
                .WithMany(f => f.Valaszok)
                .HasForeignKey(v => v.FelhasznaloId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Valasz>()
                .HasOne(v => v.Kerdes)
                .WithMany(k => k.Valaszok)
                .HasForeignKey(v => v.KerdesId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KerdesCimke>()
                .HasOne(kc => kc.Kerdes)
                .WithMany(k => k.KerdesCimkek)
                .HasForeignKey(kc => kc.KerdesId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KerdesCimke>()
                .HasOne(kc => kc.Cimke)
                .WithMany(c => c.KerdesCimkek)
                .HasForeignKey(kc => kc.CimkeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Szavazat>()
                .HasOne(s => s.Kerdes)
                .WithMany()
                .HasForeignKey(s => s.KerdesId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Szavazat>()
                .HasOne(s => s.Valasz)
                .WithMany()
                .HasForeignKey(s => s.ValaszId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Szavazat>()
                .HasOne(s => s.Felhasznalo)
                .WithMany(f => f.Szavazatok)
                .HasForeignKey(s => s.FelhasznaloId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Szavazat>()
                .HasIndex(s => new { s.FelhasznaloId, s.KerdesId })
                .HasFilter("[KerdesId] IS NOT NULL")
                .IsUnique();

            modelBuilder.Entity<Szavazat>()
                .HasIndex(s => new { s.FelhasznaloId, s.ValaszId })
                .HasFilter("[ValaszId] IS NOT NULL")
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}