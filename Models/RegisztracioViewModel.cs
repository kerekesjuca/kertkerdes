using System.ComponentModel.DataAnnotations;

namespace KertKerdes.Models
{
    public class RegisztracioViewModel
    {
        [Required(ErrorMessage = "A felhasználónév kötelező.")]
        public string Felhasznalonev { get; set; } = "";

        [Required(ErrorMessage = "Az email cím kötelező.")]
        [RegularExpression(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            ErrorMessage = "Valós email címet adj meg (pl. nev@email.com).")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "A jelszó kötelező.")]
        [MinLength(8, ErrorMessage = "A jelszó legalább 8 karakter legyen.")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "A jelszó legalább 8 karakter legyen, tartalmazzon kis- és nagybetűt, számot és speciális karaktert.")]
        public string Jelszo { get; set; } = "";

        [Required(ErrorMessage = "A jelszó ismétlése kötelező.")]
        [Compare("Jelszo", ErrorMessage = "A két jelszó nem egyezik.")]
        public string JelszoUjra { get; set; } = "";

        public string Szerepkor { get; set; } = "Altalanos";
    }
}