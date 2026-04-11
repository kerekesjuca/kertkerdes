namespace KertKerdes.Models
{
    public class Szavazat
    {
        public int Id { get; set; }

        public int FelhasznaloId { get; set; }

        public int? KerdesId { get; set; }

        public int? ValaszId { get; set; }

        public int Ertek { get; set; }

        public Felhasznalo? Felhasznalo { get; set; }

        public Kerdes? Kerdes { get; set; }

        public Valasz? Valasz { get; set; }
    }
}