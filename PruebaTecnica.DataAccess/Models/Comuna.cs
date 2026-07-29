namespace PruebaTecnica.DataAccess.Models
{
    public class Comuna
    {
        public int IdComuna { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int IdRegion { get; set; }
        public InformacionAdicional InformacionAdicional { get; set; } = new();
    }

    public class InformacionAdicional
    {
        public decimal Superficie { get; set; }

        public int Poblacion { get; set; }

        public decimal Densidad { get; set; }
    }
}