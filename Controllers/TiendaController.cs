using EjercicioTienda.Services;
using Microsoft.AspNetCore.Mvc;

namespace EjercicioTienda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TiendaController : ControllerBase
{
    private readonly IResumenService _resumen;

    public TiendaController(IResumenService resumen)
    {
        _resumen = resumen;
    }

    [HttpGet("resumen")]
    public IActionResult Resumen(decimal subtotal = 100) => Ok(_resumen.Obtener(subtotal));
}
