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
    }
}