using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KertKerdes.Models
{
    public class Kerdes
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A kérdés címe kötelező.")]
        public string Cim { get; set; } = string.Empty;

        [Required(ErrorMessage = "A kérdés leírása kötelező.")]
        public string Leiras { get; set; } = string.Empty;

        public int FelhasznaloId { get; set; }

        public Felhasznalo? Felhasznalo { get; set; }

        public int TemakorId { get; set; }

        public Temakor? Temakor { get; set; }

        public DateTime Datum { get; set; }

        public bool Jovahagyva { get; set; }

        public int Szavazat { get; set; }

        public ICollection<Valasz> Valaszok { get; set; } = new List<Valasz>();

        public ICollection<KerdesCimke> KerdesCimkek { get; set; } = new List<KerdesCimke>();
    }
}