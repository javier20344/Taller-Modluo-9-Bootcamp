using System.ComponentModel.DataAnnotations;

namespace EjercicioTienda.Options;

public class TiendaOptions
{
    public const string Seccion = "Tienda";

    [Required]
    public string NombreTienda { get; set; } = string.Empty;

    [Required]
    public string Moneda { get; set; } = string.Empty;

    [Range(0, 100)]
    public decimal IvaPorcentaje { get; set; }
}
