using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KertKerdes.Models
{
    public class Felhasznalo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A felhasználónév megadása kötelező.")]
        public string Felhasznalonev { get; set; } = string.Empty;

        [Required(ErrorMessage = "Az e-mail cím megadása kötelező.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string JelszoHash { get; set; } = string.Empty;

        [NotMapped]
        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        public string Jelszo { get; set; } = string.Empty;

        [NotMapped]
        [Required(ErrorMessage = "A jelszó megerősítése kötelező.")]
        public string JelszoUjra { get; set; } = string.Empty;

        [Required]
        public string Szerepkor { get; set; } = "Altalanos";

        public bool ModeratorJovahagyva { get; set; } = false;

        public bool ModeratorElutasitva { get; set; } = false;

        public DateTime RegisztracioDatuma { get; set; } = DateTime.Now;

        public ICollection<Kerdes> Kerdesek { get; set; } = new List<Kerdes>();

        public ICollection<Valasz> Valaszok { get; set; } = new List<Valasz>();

        public ICollection<Szavazat> Szavazatok { get; set; } = new List<Szavazat>();
    }
}