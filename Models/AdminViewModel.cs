using System.Collections.Generic;

namespace KertKerdes.Models
{
    public class AdminViewModel
    {
        public List<Kerdes> VarakozoKerdesek { get; set; } = new();

        public List<Valasz> VarakozoValaszok { get; set; } = new();

        public List<Felhasznalo> VarakozoModeratorok { get; set; } = new();
    }
}