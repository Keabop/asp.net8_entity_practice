// ============================================================================
//  CONTROLADOR: AreasController
// ----------------------------------------------------------------------------
//  En el patrón MVC (Modelo - Vista - Controlador), el CONTROLADOR es el
//  "director de orquesta". Por cada acción del usuario:
//     1. Recibe la petición (por ejemplo, "quiero ver la lista de áreas").
//     2. Habla con la base de datos a través del DbContext.
//     3. Elige una VISTA (una pantalla) y le pasa los datos para mostrarlos.
//
//  Cada MÉTODO público de esta clase es una "ACCIÓN", y normalmente
//  corresponde a una URL:
//     Index   -> GET /Areas            (lista)
//     Details -> GET /Areas/Details/5  (ver una)
//     Create  -> GET/POST /Areas/Create (formulario para crear)
//     Edit    -> GET/POST /Areas/Edit/5 (formulario para editar)
//     Delete  -> GET/POST /Areas/Delete/5 (confirmar y borrar)
//
//  Sobre "async / await" que verás mucho:
//     Hablar con la base de datos toma tiempo. En vez de dejar el hilo
//     "congelado" esperando, usamos métodos asíncronos (terminan en "Async")
//     con la palabra "await". Así el servidor puede atender a otros usuarios
//     mientras la BD responde. Es el estándar profesional; acostúmbrate a él.
// ============================================================================

using CapacitacionRH.Data;
using CapacitacionRH.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapacitacionRH.Controllers;

public class AreasController : Controller
{
    // --- Inyección de dependencias ---
    // No creamos el DbContext con "new". Lo DECLARAMOS en el constructor y la
    // app nos lo entrega ya listo (esto lo configuramos en Program.cs).
    // Lo guardamos en un campo privado "_context" para usarlo en cada acción.
    private readonly ApplicationDbContext _context;

    public AreasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ------------------------------------------------------------------ INDEX
    // GET: /Areas
    // Muestra la LISTA de todas las áreas.
    public async Task<IActionResult> Index()
    {
        // _context.Areas es nuestra tabla. ToListAsync() trae TODAS las filas.
        // EF traduce esto a un "SELECT * FROM Areas" por debajo.
        var areas = await _context.Areas.ToListAsync();

        // Pasamos la lista a la vista. La vista se llama igual que la acción
        // ("Index") y vive en /Views/Areas/Index.cshtml.
        return View(areas);
    }

    // ---------------------------------------------------------------- DETAILS
    // GET: /Areas/Details/5
    // Muestra el detalle de UNA sola área. El "int? id" viene de la URL.
    public async Task<IActionResult> Details(int? id)
    {
        // Si no llegó ningún id en la URL, no hay nada que mostrar -> 404.
        if (id == null) return NotFound();

        // FirstOrDefaultAsync busca la PRIMERA fila que cumpla la condición.
        // Si no encuentra ninguna, devuelve null (por eso el "OrDefault").
        var area = await _context.Areas
            .FirstOrDefaultAsync(a => a.Id == id);

        if (area == null) return NotFound();

        return View(area);
    }

    // ----------------------------------------------------------------- CREATE
    // GET: /Areas/Create
    // Solo MUESTRA el formulario vacío para crear un área nueva.
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Areas/Create
    // Se ejecuta cuando el usuario ENVÍA el formulario. Aquí SÍ guardamos.
    [HttpPost]                        // Esta versión responde a envíos (POST), no a la carga (GET).
    [ValidateAntiForgeryToken]        // Protección contra ataques CSRF (formularios falsificados).
    public async Task<IActionResult> Create([Bind("Nombre,Descripcion")] Area area)
    {
        // [Bind(...)] indica qué campos aceptamos del formulario. Incluimos
        // Nombre y Descripcion, pero NO el Id (ese lo genera la base de datos).
        //
        // ModelState.IsValid revisa las reglas que pusimos con Data Annotations
        // en el modelo Area ([Required], [StringLength]...). Si algo no cumple,
        // volvemos a mostrar el formulario con los mensajes de error.
        if (ModelState.IsValid)
        {
            _context.Add(area);                 // 1. "Prepara" la inserción (aún no toca la BD).
            await _context.SaveChangesAsync();  // 2. AQUÍ EF ejecuta el INSERT real en la BD.
            return RedirectToAction(nameof(Index)); // 3. Volvemos a la lista.
        }
        return View(area); // Si hubo errores, re-mostramos el form con lo que escribió.
    }

    // ------------------------------------------------------------------- EDIT
    // GET: /Areas/Edit/5
    // Muestra el formulario YA RELLENO con los datos del área a editar.
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        // FindAsync busca por clave primaria (el Id). Es la forma más directa.
        var area = await _context.Areas.FindAsync(id);
        if (area == null) return NotFound();

        return View(area);
    }

    // POST: /Areas/Edit/5
    // Guarda los cambios del formulario de edición.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion")] Area area)
    {
        // Verificamos que el id de la URL coincida con el del objeto enviado.
        if (id != area.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(area);              // Marca el objeto como "modificado".
                await _context.SaveChangesAsync();  // EF ejecuta el UPDATE en la BD.
            }
            catch (DbUpdateConcurrencyException)
            {
                // Este error ocurre si alguien borró el registro mientras lo editabas.
                // Comprobamos si el área todavía existe para dar una respuesta útil.
                if (!_context.Areas.Any(a => a.Id == area.Id))
                    return NotFound();
                throw; // Si existe pero falló por otra razón, relanzamos el error.
            }
            return RedirectToAction(nameof(Index));
        }
        return View(area);
    }

    // ----------------------------------------------------------------- DELETE
    // GET: /Areas/Delete/5
    // Muestra una pantalla de CONFIRMACIÓN antes de borrar (buena práctica:
    // nunca borres de golpe con solo un clic).
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var area = await _context.Areas
            .FirstOrDefaultAsync(a => a.Id == id);
        if (area == null) return NotFound();

        return View(area);
    }

    // POST: /Areas/Delete/5
    // Se ejecuta al pulsar "Eliminar" en la pantalla de confirmación.
    // Nota: el nombre del método es "DeleteConfirmed" pero con [ActionName("Delete")]
    // le decimos a MVC que responda a la URL /Areas/Delete. Necesitamos otro
    // nombre en C# porque no puede haber dos métodos "Delete(int? id)" iguales.
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var area = await _context.Areas.FindAsync(id);
        if (area != null)
        {
            _context.Areas.Remove(area);        // Marca la fila para borrar.
            await _context.SaveChangesAsync();  // EF ejecuta el DELETE en la BD.
        }
        return RedirectToAction(nameof(Index));
    }
}
