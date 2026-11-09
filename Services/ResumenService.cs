using EjercicioTienda.Options;
using Microsoft.Extensions.Options;

namespace EjercicioTienda.Services;

public interface IResumenService
{
    object Obtener(decimal subtotal);
}

public class ResumenService : IResumenService
{
    private readonly IOptionsSnapshot<TiendaOptions> _opciones;
    private readonly ICalculadoraIva _calculadora;

    public ResumenService(IOptionsSnapshot<TiendaOptions> opciones, ICalculadoraIva calculadora)
    {
        _opciones = opciones;
        _calculadora = calculadora;
    }

    public object Obtener(decimal subtotal) => new
    {
        tienda = _opciones.Value.NombreTienda,
        moneda = _opciones.Value.Moneda,
        subtotal,
        total = _calculadora.Calcular(subtotal)
    };
}
