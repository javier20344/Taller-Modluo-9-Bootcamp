using EjercicioTienda.Options;
using Microsoft.Extensions.Options;

namespace EjercicioTienda.Services;

public interface ICalculadoraIva
{
    decimal Calcular(decimal subtotal);
}

public class CalculadoraIva : ICalculadoraIva
{
    private readonly TiendaOptions _opciones;

    public CalculadoraIva(IOptions<TiendaOptions> opciones)
    {
        _opciones = opciones.Value;
    }

    public decimal Calcular(decimal subtotal) =>
        subtotal + (subtotal * _opciones.IvaPorcentaje / 100);
}
