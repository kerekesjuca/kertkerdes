using System;

namespace KertKerdes.Models
{
    public class Valasz
    {
        public int Id { get; set; }

        public string Szoveg { get; set; }

        public string Szerzo { get; set; }

        public DateTime Datum { get; set; } = DateTime.Now;

        public bool Jovahagyva { get; set; } = false;

        public bool Elfogadott { get; set; } = false;

        public int Szavazat { get; set; } = 0;

        public int KerdesId { get; set; }

        public Kerdes Kerdes { get; set; }
    }
}