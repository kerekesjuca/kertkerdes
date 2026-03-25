namespace KertKerdes.Models
{
    public class Szavazat
    {
        public int Id { get; set; }

        public int FelhasznaloId { get; set; }

        public int? KerdesId { get; set; }

        public int? ValaszId { get; set; }

        public int Ertek { get; set; }
    }
}