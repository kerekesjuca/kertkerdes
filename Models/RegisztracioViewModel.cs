using System.ComponentModel.DataAnnotations;

namespace KertKerdes.Models
{
    public class RegisztracioViewModel
    {
        [Required]
        public string Felhasznalonev { get; set; } = "";

        [Required]
        [RegularExpression(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            ErrorMessage = "Valós email címet adj meg (pl. nev@email.com).")]
        public string Email { get; set; } = "";

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "A jelszó legalább 8 karakter legyen, tartalmazzon kis- és nagybetűt, számot és speciális karaktert.")]
        public string Jelszo { get; set; } = "";

        [Required]
        [Compare("Jelszo", ErrorMessage = "A két jelszó nem egyezik.")]
        public string JelszoUjra { get; set; } = "";

        public string Szerepkor { get; set; } = "Altalanos";
    }
}