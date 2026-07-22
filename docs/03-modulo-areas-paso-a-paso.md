# 03 · Módulo "Áreas", paso a paso

Aquí juntamos todo lo de las lecciones 01 y 02 en un módulo **real y completo**:
el CRUD de Áreas. **CRUD** son las 4 operaciones básicas sobre cualquier dato:

| CRUD | Español | Verbo HTTP | Método EF |
|------|---------|-----------|-----------|
| **C**reate | Crear | POST | `Add` |
| **R**ead | Leer / consultar | GET | `ToList` / `Find` |
| **U**pdate | Actualizar | POST | `Update` |
| **D**elete | Borrar | POST | `Remove` |

Sigue el recorrido con los archivos abiertos al lado.

---

## Pieza 1 · El modelo — `Models/Area.cs`

Define QUÉ es un área y sus reglas. Lo esencial:

```csharp
public class Area
{
    public int Id { get; set; }                    // Clave primaria (autoincremental).

    [Required, StringLength(100)]
    public string Nombre { get; set; } = "";       // Obligatorio, máx 100.

    [StringLength(300)]
    public string? Descripcion { get; set; }        // Opcional (el "?" permite null).
}
```

- `Id` → EF lo detecta como clave primaria por su nombre.
- Los atributos `[...]` (**Data Annotations**) sirven **doble**: validan el
  formulario Y definen la columna en la BD.

---

## Pieza 2 · El puente — `Data/ApplicationDbContext.cs`

Registra la tabla:

```csharp
public DbSet<Area> Areas => Set<Area>();
```

Con esto, `_context.Areas` es tu puerta a la tabla `Areas`.

---

## Pieza 3 · El enganche — `Program.cs`

Dos líneas conectan EF con la app:

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(connectionString));
```

Esto hace que cualquier controlador pueda **pedir** un `ApplicationDbContext` en
su constructor y recibirlo listo. A ese mecanismo se le llama **inyección de
dependencias**.

---

## Pieza 4 · El cerebro — `Controllers/AreasController.cs`

Cada acción es un método. Los tres patrones que se repiten:

### Leer una lista (READ)
```csharp
public async Task<IActionResult> Index()
{
    var areas = await _context.Areas.ToListAsync();  // SELECT * FROM Areas
    return View(areas);                              // se la paso a la vista
}
```

### Guardar algo nuevo (CREATE)
Ojo: **dos** métodos con el mismo nombre `Create`.
- El `GET` (sin `[HttpPost]`) solo **muestra** el formulario vacío.
- El `POST` (con `[HttpPost]`) **recibe** los datos y los guarda:

```csharp
[HttpPost]
[ValidateAntiForgeryToken]     // seguridad anti-CSRF (formularios falsificados)
public async Task<IActionResult> Create([Bind("Nombre,Descripcion")] Area area)
{
    if (ModelState.IsValid)                  // ¿cumple las reglas del modelo?
    {
        _context.Add(area);                  // preparar INSERT
        await _context.SaveChangesAsync();   // ejecutar INSERT
        return RedirectToAction(nameof(Index)); // volver a la lista
    }
    return View(area);                       // si hubo errores, re-mostrar el form
}
```

Fíjate en el trío mágico: `Add` → `SaveChangesAsync` → `RedirectToAction`.
Ese patrón (**guardar y redirigir**) evita que, si el usuario recarga la página,
se vuelva a enviar el formulario por accidente.

### Editar y Borrar
Siguen la misma idea: un `GET` que muestra (el registro ya cargado desde la BD)
y un `POST` que confirma el cambio (`Update` o `Remove` + `SaveChangesAsync`).
Para Borrar, primero mostramos una **pantalla de confirmación**: nunca borres
con un solo clic sin avisar.

---

## Pieza 5 · Las pantallas — `Views/Areas/*.cshtml`

Escritas en **Razor** = HTML + C# usando `@`. Lo más importante son los
**Tag Helpers** (atributos `asp-*`), que generan HTML conectado a tu modelo:

| Tag Helper | Qué hace |
|-----------|----------|
| `asp-for="Nombre"` | Genera el `<input>`/`<label>` correcto para la propiedad `Nombre`, con su validación. |
| `asp-validation-for="Nombre"` | Muestra el mensaje de error de ese campo. |
| `asp-action="Create"` | Genera el enlace/envío hacia la acción `Create`. |
| `asp-route-id="@area.Id"` | Añade el `id` a la URL (ej. `/Areas/Edit/3`). |

Ejemplo del formulario (`Create.cshtml`):

```html
<form asp-action="Create" method="post">
    <label asp-for="Nombre"></label>
    <input asp-for="Nombre" class="form-control" />
    <span asp-validation-for="Nombre" class="text-danger"></span>
    <button type="submit">Guardar</button>
</form>
```

No escribimos el `name=""` ni las reglas de validación a mano: los Tag Helpers
los sacan del modelo `Area`. **El modelo es la única fuente de la verdad.**

---

## Pieza 6 · El menú — `Views/Shared/_Layout.cshtml`

`_Layout.cshtml` es la **plantilla común** (encabezado, menú, pie) que envuelve
todas las páginas. Ahí agregamos el enlace a "Áreas":

```html
<a class="nav-link" asp-controller="Areas" asp-action="Index">Áreas</a>
```

---

## El recorrido completo, ya con nombres reales

Cuando en el navegador creas el área "Producción":

```
1. GET  /Areas/Create      → AreasController.Create()  muestra el form vacío.
2. Llenas "Producción" y das Guardar.
3. POST /Areas/Create      → AreasController.Create(area) recibe los datos.
4. ModelState.IsValid comprueba [Required], [StringLength]... ✔
5. _context.Add(area) + SaveChangesAsync() → INSERT en SQLite.
6. RedirectToAction(Index) → el navegador va a /Areas.
7. GET  /Areas             → Index() hace SELECT y la vista lista "Producción".
```

¡Eso es un CRUD completo! **Todos los módulos que siguen (Puestos, Trabajadores,
Cursos) son este mismo patrón**, sumándole relaciones entre tablas.

---

### ✅ Lo que deberías llevarte de esta lección

- Un **CRUD** = Crear, Leer, Actualizar, Borrar; es el 80% de una app de datos.
- El patrón se repite: **GET muestra**, **POST guarda** (`SaveChangesAsync`) y
  **redirige**.
- Los **Tag Helpers** (`asp-*`) conectan las vistas con el modelo sin código
  manual.
- Dominado este módulo, los demás son "más de lo mismo" + relaciones.

➡️ Siguiente: [04 · Cómo ejecutar el proyecto](04-como-ejecutar.md)
