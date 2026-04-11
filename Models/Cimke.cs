using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KertKerdes.Models
{
    public class Cimke
    {
        public int Id { get; set; }

        [Required]
        public string Nev { get; set; } = string.Empty;

        public ICollection<KerdesCimke> KerdesCimkek { get; set; } = new List<KerdesCimke>();
    }
}