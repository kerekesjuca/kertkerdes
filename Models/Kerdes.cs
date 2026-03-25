using System.ComponentModel.DataAnnotations;

namespace KertKerdes.Models
{
    public class Kerdes
    {
        public int Id { get; set; }

        [Required]
        public string Cim { get; set; } = string.Empty;

        [Required]
        public string Leiras { get; set; } = string.Empty;

        public int FelhasznaloId { get; set; }

        public int TemakorId { get; set; }

        public DateTime Datum { get; set; }

        public bool Jovahagyva { get; set; }

        public int Szavazat { get; set; }

        public List<Valasz> Valaszok { get; set; } = new();

        public List<KerdesCimke> KerdesCimkek { get; set; } = new();
    }
}