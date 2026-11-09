# Ejercicio: la tienda que no arranca

**Tiempo:** 20 minutos. **Temas:** inyección de dependencias y configuración.

Este proyecto tiene errores a propósito. Tu tarea es hacer que funcione.

## Reglas

1. Ejecuta `dotnet run` y lee el error completo.
2. Anota qué falta o qué está mal, y quién lo necesita.
3. Corrige solo la causa de ese error y vuelve a ejecutar.
4. Repite hasta que la API arranque.
5. No cambies las reglas de validación de `TiendaOptions`.

## Resultado esperado

Llama a `GET http://localhost:5000/api/tienda/resumen?subtotal=100` y recibe:

```json
{ "tienda": "Tienda Bootcamp", "moneda": "USD", "subtotal": 100, "total": 115 }
```

## Entrega

Completa una fila por cada error que encuentres:

| # | Mensaje principal | Causa | Corrección |
|---|---|---|---|
| 1 |Microsoft.Extensions.Options.OptionsValidationException: DataAnnotation validation failed for 'TiendaOptions' members: 'NombreTienda', 'Moneda' | 
Program.cs busca la sección "Tienda", pero en appsettings.json la clave se llamó "Tiendas" (con s). Al no encontrarla, las propiedades requeridas [Required] quedan nulas.| 
En appsettings.json, renombrar la clave "Tiendas" a "Tienda". | 

| 2 |Microsoft.Extensions.Options.OptionsValidationException: DataAnnotation validation failed for 'TiendaOptions' member: 'IvaPorcentaje' with error: 'The field IvaPorcentaje must be between 0 and 100.'| 
En appsettings.json, la propiedad "IvaPorcentaje" tiene el valor 150, violando el atributo [Range(0, 100)] de TiendaOptions.cs.|
En appsettings.json, cambiar "IvaPorcentaje": 150 por "IvaPorcentaje": 15.| 

| 3 |System.InvalidOperationException: Unable to resolve service for type 'EjercicioTienda.Services.ICalculadoraIva' while attempting to activate 'EjercicioTienda.Services.ResumenService'.| 
ResumenService requiere ICalculadoraIva en su constructor, pero el servicio CalculadoraIva no se registró en el contenedor de dependencias de Program.cs.|
En Program.cs, agregar: builder.Services.AddScoped<ICalculadoraIva, CalculadoraIva>();|

| 4 | System.InvalidOperationException: Cannot consume scoped service 'Microsoft.Extensions.Options.IOptionsSnapshot<EjercicioTienda.Options.TiendaOptions>' from singleton 'EjercicioTienda.Services.IResumenService'.|
Dependencia Cautiva: ResumenService está registrado como Singleton, pero inyecta IOptionsSnapshot<TiendaOptions>, que tiene un ciclo de vida Scoped.|
En Program.cs, cambiar AddSingleton por AddScoped:builder.Services.AddScoped<IResumenService, ResumenService>();|

## Pregunta final

¿Por qué `ResumenService` no puede ser Singleton mientras use `IOptionsSnapshot`?
IOptionsSnapshot es un servicio con ciclo de vida Scoped (se crea una nueva instancia por cada petición HTTP para permitir recalcular opciones o reflejar cambios en tiempo real). Por su parte, un servicio Singleton se crea una sola vez y vive durante toda la ejecución de la aplicación.

Si un servicio Singleton inyecta un servicio Scoped, ocurre el anti-patrón de Dependencia Cautiva (Captive Dependency): el Singleton se queda reteniendo la primera instancia del IOptionsSnapshot de por vida, impidiendo que la configuración vuelva a actualizarse en peticiones posteriores. El motor de inyección de dependencias de .NET detecta este conflicto de ciclos de vida durante la validación de scopes y lanza una excepción para evitarlo.