using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KertKerdes.Models
{
    public class Kerdes
    {
        public int Id { get; set; }

        [Required]
        public string Cim { get; set; }

        [Required]
        public string Leiras { get; set; }

        public string Temakor { get; set; }

        public string Szerzo { get; set; }

        public DateTime Datum { get; set; } = DateTime.Now;

        public bool Jovahagyva { get; set; } = false;

        public int Szavazat { get; set; } = 0;

        public List<Valasz> Valaszok { get; set; } = new();
    }
}