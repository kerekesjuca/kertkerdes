using System.ComponentModel.DataAnnotations;

namespace KertKerdes.Models
{
    public class BejelentkezesViewModel
    {
        [Required(ErrorMessage = "Az e-mail cím megadása kötelező.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        public string Jelszo { get; set; } = "";
    }
}