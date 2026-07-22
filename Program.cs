// ============================================================================
//  Program.cs  ->  EL PUNTO DE ARRANQUE de toda la aplicación
// ----------------------------------------------------------------------------
//  Cuando ejecutas la app, la PRIMERA línea de código que corre está aquí.
//  Este archivo hace DOS cosas, en dos "bloques":
//
//    BLOQUE 1 (antes de builder.Build()):  REGISTRAR SERVICIOS.
//        Le decimos a la app qué herramientas tendrá disponibles:
//        controladores, vistas, la conexión a la base de datos, etc.
//        A esto se le llama "inyección de dependencias" (Dependency Injection).
//
//    BLOQUE 2 (después de builder.Build()): ARMAR EL "PIPELINE".
//        Definimos la CADENA DE PASOS por la que pasa cada petición web (HTTP)
//        que llega. Cada "app.UseXxx(...)" es un eslabón de esa cadena, y el
//        ORDEN importa mucho (se ejecutan de arriba hacia abajo).
// ============================================================================

using CapacitacionRH.Data;             // Nuestro ApplicationDbContext
using Microsoft.EntityFrameworkCore;   // Para UseSqlite(...)

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------
// BLOQUE 1: SERVICIOS
// ---------------------------------------------------------------------------

// Habilita el patrón MVC (Controladores + Vistas). Ya venía en la plantilla.
builder.Services.AddControllersWithViews();

// Leemos la "cadena de conexión" desde el archivo appsettings.json.
// Una cadena de conexión es simplemente el texto que dice DÓNDE está la base
// de datos y CÓMO conectarse. La nuestra apunta a un archivo SQLite local.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registramos nuestro ApplicationDbContext como un servicio.
// A partir de aquí, cualquier controlador puede "pedir" un ApplicationDbContext
// en su constructor y la app se lo entregará ya listo y conectado.
//   - UseSqlite(...) elige el PROVEEDOR de base de datos: SQLite.
//     (Si mañana la empresa usa SQL Server, cambiarías esta sola línea por
//      UseSqlServer(...) y agregarías su paquete: el resto del código no cambia.
//      Ese es otro superpoder de EF: es casi independiente del motor de BD.)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// ---------------------------------------------------------------------------
// BLOQUE 2: PIPELINE (la cadena por la que pasa cada petición)
// ---------------------------------------------------------------------------

// Si NO estamos en modo desarrollo (es decir, en producción), mostramos una
// página de error genérica y activamos seguridad extra (HSTS). En desarrollo
// preferimos ver el error detallado para poder depurar.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();  // Redirige http:// a https:// (conexión segura).
app.UseStaticFiles();       // Permite servir archivos de wwwroot (css, js, imágenes).

app.UseRouting();           // Decide QUÉ controlador/acción atiende cada URL.

app.UseAuthorization();     // Comprueba permisos (lo usaremos más adelante con login).

// Regla de ruteo por defecto. Traduce una URL en:  Controlador / Acción / id.
//   Ejemplos:
//     /                    -> HomeController,  acción Index
//     /Areas               -> AreasController, acción Index (listar áreas)
//     /Areas/Edit/3        -> AreasController, acción Edit, con id = 3
// Los "=Home", "=Index" son los valores por defecto si la URL viene incompleta.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();  // Arranca el servidor web y se queda "escuchando" peticiones.
