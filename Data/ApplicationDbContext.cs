// ============================================================================
//  DbContext: ApplicationDbContext
// ----------------------------------------------------------------------------
//  El "DbContext" es la pieza CENTRAL de Entity Framework Core.
//  Piénsalo como el "puente" entre tu código C# y la base de datos:
//
//      Tu código  <----  ApplicationDbContext  ---->  Base de datos (SQLite)
//
//  Sus dos trabajos principales:
//    1. Saber QUÉ tablas existen  -> mediante las propiedades "DbSet<...>".
//    2. Traducir tus instrucciones de C# (agregar, buscar, borrar...) a
//       comandos SQL reales que la base de datos entiende.
//
//  Tú NUNCA escribes SQL a mano: le hablas a este objeto en C# y EF genera
//  el SQL por ti. Ese es el gran superpoder de un ORM (Object-Relational
//  Mapper), que es la categoría de herramienta a la que pertenece EF Core.
// ============================================================================

using CapacitacionRH.Models;          // Para conocer nuestras clases modelo (Area, etc.)
using Microsoft.EntityFrameworkCore;  // El corazón de EF Core (DbContext, DbSet...)

namespace CapacitacionRH.Data;

// Heredamos de "DbContext": esa clase base de EF ya trae toda la maquinaria
// (abrir conexión, generar SQL, seguir los cambios de tus objetos, etc.).
// Nosotros solo la extendemos para decirle CUÁLES son NUESTRAS tablas.
public class ApplicationDbContext : DbContext
{
    // --- Constructor ---
    // Cuando la aplicación arranca, en Program.cs le decimos a EF cómo
    // conectarse a la base de datos (qué proveedor, qué cadena de conexión).
    // Esa configuración llega aquí a través de "options" y se la pasamos a la
    // clase base con ": base(options)". No necesitas tocar esto casi nunca.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // --- DbSet: una propiedad por cada TABLA ---
    // "DbSet<Area>" significa: "existe una tabla en la BD cuyas filas son
    // objetos de tipo Area". El nombre que le des a la propiedad ("Areas")
    // será, por convención, el nombre de la tabla.
    //
    // A través de esta propiedad harás TODO con las áreas, por ejemplo:
    //     _context.Areas.ToList();          -> traer todas las áreas
    //     _context.Areas.Add(nuevaArea);    -> preparar una para insertar
    //     _context.Areas.Find(5);           -> buscar el área con Id = 5
    //
    // Conforme avancemos en el curso agregaremos aquí más DbSet:
    // Puestos, Trabajadores, Cursos... uno por cada tabla nueva.
    public DbSet<Area> Areas => Set<Area>();
}
