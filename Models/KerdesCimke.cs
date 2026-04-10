namespace KertKerdes.Models
{
    public class KerdesCimke
    {
        public int Id { get; set; }

        public int KerdesId { get; set; }
        public Kerdes? Kerdes { get; set; }

        public int CimkeId { get; set; }
        public Cimke? Cimke { get; set; }

    }
}