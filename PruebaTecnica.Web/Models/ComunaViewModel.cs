namespace PruebaTecnica.Web.Models;

public class ComunaViewModel
{
    public int IdComuna { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int IdRegion { get; set; }

    public InformacionAdicionalViewModel InformacionAdicional { get; set; } = new();
}