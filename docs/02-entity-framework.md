# 02 · Entity Framework Core

Esta es la parte que más te importa para tu proyecto de empresa. Tómatela con
calma: entender EF bien te ahorrará meses de dolores de cabeza.

---

## 1. El problema que resuelve EF

Una base de datos guarda datos en **tablas** (filas y columnas), y para hablar
con ella se usa un lenguaje llamado **SQL**:

```sql
SELECT * FROM Areas WHERE Id = 5;
INSERT INTO Areas (Nombre, Descripcion) VALUES ('Calidad', 'Control de calidad');
```

Pero en tu app trabajas con **objetos de C#** (`Area`, `Trabajador`...). Habría
que estar traduciendo a mano, todo el tiempo, entre "objetos C#" y "filas SQL".
Eso es tedioso y propenso a errores.

**Entity Framework Core** es un **ORM** (Object-Relational Mapper): un traductor
automático entre esos dos mundos.

```
   Tus objetos C#      ◄──── Entity Framework ────►      Tablas SQL
   (Area, Trabajador)         (el traductor)          (filas y columnas)
```

Tú escribes C# normal, y EF genera el SQL por ti:

```csharp
// TÚ escribes esto (C#, natural y legible):
var area = await _context.Areas.FindAsync(5);

// EF ejecuta esto por debajo (SQL, tú no lo ves):
// SELECT * FROM Areas WHERE Id = 5;
```

---

## 2. Las tres piezas de EF

### a) La **entidad** (tu modelo) → una tabla

Una clase de C# donde **cada propiedad es una columna**. Ya la tienes:
`Models/Area.cs`. Recuerda la traducción:

| C# | Base de datos |
|----|---------------|
| `class Area` | tabla `Areas` |
| un objeto `Area` | una fila |
| `public string Nombre` | columna `Nombre` |
| `public int Id` | columna `Id` (clave primaria) |

### b) El **DbContext** → el puente

`Data/ApplicationDbContext.cs` es tu conexión y catálogo de tablas. Cada
`DbSet<T>` es una tabla:

```csharp
public DbSet<Area> Areas => Set<Area>();   // ← "existe la tabla Areas"
```

A través de él haces TODO:

```csharp
_context.Areas.ToListAsync();        // SELECT  (traer todas)
_context.Areas.FindAsync(5);         // SELECT WHERE Id=5
_context.Add(area);                  // preparar un INSERT
_context.Update(area);               // preparar un UPDATE
_context.Remove(area);               // preparar un DELETE
await _context.SaveChangesAsync();   // ← EJECUTA de verdad lo preparado
```

> ⚠️ Detalle clave: `Add`, `Update` y `Remove` solo **anotan** la intención en
> memoria. Nada toca la base de datos hasta que llamas a **`SaveChangesAsync()`**.
> Es como un carrito de compras: agregas cosas, pero solo se cobra al pagar.

### c) Las **migraciones** → el historial de la estructura

Usamos el enfoque **"Code First"**: tú defines las clases en C#, y EF crea/actualiza
las tablas. Una **migración** es un archivo que describe *un cambio* en la
estructura de la BD.

---

## 3. El ciclo de las migraciones (¡esto lo usarás siempre!)

Cada vez que agregues o cambies una entidad, repites estos dos comandos:

```bash
# 1. EF compara tus clases actuales con la última migración y GENERA el cambio.
#    "NombreDescriptivo" es como el mensaje de un commit: describe el cambio.
dotnet ef migrations add NombreDescriptivo

# 2. EF APLICA las migraciones pendientes a la base de datos real.
dotnet ef database update
```

Lo que pasó la primera vez, con nuestra `Area`:

1. `migrations add MigracionInicial` generó `Migrations/2026..._MigracionInicial.cs`
   con un método `Up()` que dice "crea la tabla Areas con estas columnas".
2. `database update` ejecutó ese `Up()` → creó el archivo `CapacitacionRH.db`
   con la tabla dentro.

Mira el método `Up()` generado (resumido):

```csharp
migrationBuilder.CreateTable(
    name: "Areas",
    columns: table => new
    {
        Id = table.Column<int>(nullable: false).Annotation("Sqlite:Autoincrement", true),
        Nombre = table.Column<string>(maxLength: 100, nullable: false),  // por [Required] + [StringLength(100)]
        Descripcion = table.Column<string>(maxLength: 300, nullable: true) // por "string?"
    },
    constraints: table => { table.PrimaryKey("PK_Areas", x => x.Id); });
```

¿Ves cómo tus **Data Annotations** del modelo se convirtieron en reglas de la
tabla? `[Required]` → `nullable: false`. `[StringLength(100)]` → `maxLength: 100`.
El `string?` (con `?`) → `nullable: true`. **El modelo manda; la BD obedece.**

> Cada migración tiene también un `Down()` que **revierte** el cambio (borra la
> tabla). Por eso puedes "viajar en el tiempo" con
> `dotnet ef database update NombreDeUnaMigracionAnterior`.

---

## 4. ¿Por qué SQLite aquí y SQL Server en la empresa?

- **SQLite** (lo que usamos): toda la base de datos es UN archivo (`.db`). No
  necesitas instalar nada. Perfecto para aprender y para prototipos.
- **SQL Server / PostgreSQL / MySQL** (lo típico en empresas): un servidor de
  base de datos dedicado, más potente, con usuarios, respaldos, etc.

**La gran ventaja de EF:** cambiar de motor es casi trivial. En `Program.cs`:

```csharp
options.UseSqlite(connectionString);       // hoy, para aprender
// options.UseSqlServer(connectionString); // mañana, en la empresa
```

Cambias esa línea, agregas el paquete del proveedor, ajustas la cadena de
conexión... y **el resto de tu código (modelos, controladores, consultas) no
cambia nada**. Ese es el valor de programar contra un ORM.

---

### ✅ Lo que deberías llevarte de esta lección

- EF Core es un **traductor** (ORM) entre objetos C# y tablas SQL: tú no
  escribes SQL a mano.
- **Entidad** = tabla, **DbContext** = puente, **DbSet** = una tabla.
- Los cambios en memoria (`Add/Update/Remove`) se confirman con
  **`SaveChangesAsync()`**.
- **Code First**: defines clases → `migrations add` → `database update`. Ese
  ciclo lo repetirás toda tu vida con EF.
- Tus **Data Annotations** dictan cómo queda la tabla.

➡️ Siguiente: [03 · Módulo Áreas, paso a paso](03-modulo-areas-paso-a-paso.md)
