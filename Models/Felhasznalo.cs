using System;
using System.Collections.Generic;

namespace KertKerdes.Models
{
    public class Felhasznalo
    {
        public int Id { get; set; }

        public string Felhasznalonev { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string JelszoHash { get; set; } = string.Empty;

        public string Szerepkor { get; set; } = "Felhasznalo";

        public DateTime RegisztracioDatuma { get; set; } = DateTime.Now;

        public List<Kerdes> Kerdesek { get; set; } = new();

        public List<Valasz> Valaszok { get; set; } = new();
    }
}