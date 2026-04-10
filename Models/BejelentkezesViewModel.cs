using System.ComponentModel.DataAnnotations;

namespace KertKerdes.Models
{
    public class BejelentkezesViewModel
    {
        [Required]
        public string Email { get; set; } = "";

        [Required]
        public string Jelszo { get; set; } = "";
    }
}