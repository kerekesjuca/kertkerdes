using System;
using System.ComponentModel.DataAnnotations;

namespace KertKerdes.Models
{
    public class Felhasznalo
    {
        public int Id { get; set; }

        [Required]
        public string Felhasznalonev { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";

        [Required]
        public string JelszoHash { get; set; } = "";

        [Required]
        public string Szerepkor { get; set; } = "Altalanos";

        public bool ModeratorJovahagyva { get; set; } = false;

        public bool ModeratorElutasitva { get; set; } = false;

        public DateTime RegisztracioDatuma { get; set; } = DateTime.Now;
    }
}