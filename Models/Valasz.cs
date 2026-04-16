using System;
using System.ComponentModel.DataAnnotations;

namespace KertKerdes.Models
{
    public class Valasz
    {
        public int Id { get; set; }

        public int KerdesId { get; set; }

        public int FelhasznaloId { get; set; }

        [Required(ErrorMessage = "A válasz szövege kötelező.")]
        public string Szoveg { get; set; } = string.Empty;

        public DateTime Datum { get; set; }

        public bool Jovahagyva { get; set; }

        public bool Elfogadott { get; set; }

        public int Szavazat { get; set; }

        public Kerdes? Kerdes { get; set; }

        public Felhasznalo? Felhasznalo { get; set; }
    }
}