using EjercicioTienda.Options;
using EjercicioTienda.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOptions<TiendaOptions>()
    .Bind(builder.Configuration.GetSection(TiendaOptions.Seccion))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// aqui el error era que estaba como singleton, y se cambio a scoped 
builder.Services.AddScoped<ICalculadoraIva, CalculadoraIva>();
builder.Services.AddScoped<IResumenService, ResumenService>();

var app = builder.Build();

app.MapControllers();
app.Run();
