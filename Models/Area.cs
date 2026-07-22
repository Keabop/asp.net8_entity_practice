// ============================================================================
//  MODELO: Area
// ----------------------------------------------------------------------------
//  Un "modelo" es una clase de C# que representa un concepto de tu negocio.
//  En nuestro sistema de RH/Capacitación, un "Área" es una zona de la planta
//  (por ejemplo: "Producción", "Almacén", "Calidad", "Mantenimiento").
//
//  Con Entity Framework Core usamos el enfoque "Code First": tú escribes la
//  clase en C#, y EF se encarga de crear la TABLA equivalente en la base de
//  datos. Cada PROPIEDAD de la clase se convierte en una COLUMNA de la tabla.
//
//  Traducción mental:
//      clase  C#     ->  tabla   en la base de datos
//      objeto en C#   ->  fila    (registro) en la tabla
//      propiedad C#   ->  columna de la tabla
// ============================================================================

// Este "using" nos da acceso a las "Data Annotations": unos atributos que
// escribimos entre corchetes [ ] encima de las propiedades para darle reglas
// e instrucciones a EF y a los formularios (por ejemplo, "este campo es
// obligatorio" o "máximo 100 caracteres").
using System.ComponentModel.DataAnnotations;

namespace CapacitacionRH.Models;

public class Area
{
    // --- Clave primaria (Primary Key) ---
    // Toda tabla necesita una columna que identifique de forma ÚNICA cada fila.
    // EF Core sigue una convención: si una propiedad se llama "Id" (o "AreaId"),
    // la toma automáticamente como clave primaria y, además, la configura como
    // AUTOINCREMENTAL (la base de datos le asigna 1, 2, 3... sola).
    // Por eso NO necesitamos escribir el Id al crear un Área: la BD lo pone.
    public int Id { get; set; }

    // --- Nombre del área ---
    // [Required]      -> el campo es OBLIGATORIO. Si el usuario lo deja vacío,
    //                    el formulario mostrará un error y no guardará.
    // [StringLength]  -> limita el largo máximo del texto (aquí, 100 caracteres).
    //                    Esto también le dice a la BD que la columna sea VARCHAR(100).
    // [Display]       -> el texto "bonito" que se mostrará como etiqueta en las
    //                    pantallas (en vez del nombre técnico de la propiedad).
    [Required(ErrorMessage = "El nombre del área es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    [Display(Name = "Nombre del área")]
    public string Nombre { get; set; } = string.Empty;
    //                                    ^^^^^^^^^^^^^^^
    // Inicializamos en cadena vacía para evitar advertencias de "null".
    // (El proyecto tiene activado <Nullable>enable>, que nos avisa cuando una
    //  variable de texto podría quedar en null; darle un valor por defecto la
    //  mantiene "no nula" y tranquila al compilador.)

    // --- Descripción (opcional) ---
    // Fíjate en el signo de interrogación: "string?".
    // Ese "?" significa que la propiedad PUEDE ser null (puede quedar vacía).
    // Así le decimos a EF que esta columna admite valores nulos (es opcional).
    [StringLength(300)]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }
}
