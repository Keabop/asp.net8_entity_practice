# 01 · Fundamentos de ASP.NET Core MVC

Antes del código, cuatro ideas que lo explican todo. Léelas con calma.

---

## 1. ¿Qué es una aplicación web, en el fondo?

Cuando abres una página, tu navegador (el **cliente**) le manda un mensaje a un
**servidor** pidiendo algo. El servidor procesa esa petición y devuelve una
respuesta (normalmente HTML, que el navegador dibuja).

```
   NAVEGADOR  ── petición  (GET /Areas) ──►  SERVIDOR (tu app ASP.NET)
   NAVEGADOR  ◄── respuesta (HTML)      ──   SERVIDOR
```

A ese diálogo se le llama **HTTP**. Los dos verbos que más usarás:

- **GET**  → "dame/muéstrame algo" (abrir una página, ver una lista).
- **POST** → "aquí van datos, guárdalos" (enviar un formulario).

**ASP.NET Core** es el framework de Microsoft para escribir ese "servidor" en el
lenguaje **C#**. Es rápido, gratuito, multiplataforma (corre en Windows, Linux,
Mac) y muy usado en empresas.

---

## 2. El patrón MVC (Modelo · Vista · Controlador)

MVC es una forma de **organizar** el código separando responsabilidades. Piensa
en un restaurante:

| Pieza | En el restaurante | En el código | En nuestro proyecto |
|-------|-------------------|--------------|---------------------|
| **Modelo** | Los ingredientes / la receta | Los **datos** y sus reglas | `Models/Area.cs` |
| **Vista** | El plato emplatado que ves | La **pantalla** (HTML) | `Views/Areas/Index.cshtml` |
| **Controlador** | El **mesero** que coordina | La **lógica** que une todo | `Controllers/AreasController.cs` |

El **controlador** es el coordinador: recibe tu pedido, va por los datos
(modelo) y te trae la pantalla (vista). Nunca mezclamos todo en un solo archivo:
cada quien tiene su trabajo. Eso hace el código fácil de entender y de mantener
—clave en un proyecto grande de empresa.

---

## 3. El viaje de una petición (paso a paso)

Sigamos qué pasa cuando entras a `http://localhost:5080/Areas`:

```
1. Escribes /Areas en el navegador y presionas Enter.        (petición GET)
                     │
2. Program.cs tiene el "pipeline": la petición pasa por
   varios filtros (archivos estáticos, ruteo, permisos...).
                     │
3. El RUTEO lee la URL "/Areas" y decide:
   Controlador = "Areas",  Acción = "Index"  (valor por defecto).
                     │
4. Se ejecuta AreasController.Index().
   Ese método le pide al DbContext la lista de áreas.
                     │
5. El DbContext (Entity Framework) genera un SELECT, lo manda
   a SQLite y devuelve las filas convertidas en objetos Area.
                     │
6. El controlador entrega esos objetos a la VISTA Index.cshtml.
                     │
7. La vista mezcla los datos con HTML y produce la página final.
                     │
8. El servidor devuelve ese HTML al navegador, que lo dibuja.   (respuesta)
```

Memoriza este círculo: **URL → Controlador → Modelo/BD → Vista → HTML**.
Todo en ASP.NET MVC es una variación de este mismo baile.

---

## 4. La estructura de carpetas del proyecto

Abre la raíz del proyecto y verás esto (te explico lo importante):

```
CapacitacionRH/
│
├── Program.cs              ← Punto de ARRANQUE. Configura servicios y el pipeline.
├── appsettings.json        ← Configuración (ej: la cadena de conexión a la BD).
├── CapacitacionRH.csproj   ← "Ficha técnica" del proyecto: versión .NET y paquetes.
│
├── Models/                 ← Las CLASES de datos (Area, y pronto Puesto, Trabajador...).
│   └── Area.cs
│
├── Data/                   ← El "puente" a la base de datos.
│   └── ApplicationDbContext.cs
│
├── Controllers/            ← La LÓGICA. Un controlador por módulo.
│   ├── HomeController.cs
│   └── AreasController.cs
│
├── Views/                  ← Las PANTALLAS (archivos .cshtml = HTML + C#).
│   ├── Shared/_Layout.cshtml   ← La plantilla común (menú, pie de página).
│   ├── Home/
│   └── Areas/                   ← Una carpeta por controlador.
│       ├── Index.cshtml
│       ├── Create.cshtml
│       ├── Edit.cshtml
│       ├── Details.cshtml
│       └── Delete.cshtml
│
├── Migrations/             ← El "historial" de cambios de la BD (lo crea EF).
│
└── wwwroot/                ← Archivos que el navegador descarga tal cual:
    ├── css/                   CSS, JavaScript, imágenes, y librerías como
    ├── js/                    Bootstrap (que da el estilo visual) y jQuery.
    └── lib/
```

**Convención de oro de MVC**: el controlador `AreasController`, en su acción
`Index()`, busca automáticamente la vista en `Views/Areas/Index.cshtml`. El
nombre de la carpeta = nombre del controlador (sin la palabra "Controller"), y
el nombre del archivo = nombre de la acción. Si respetas esa convención, todo
"se encuentra solo" sin que tengas que configurar nada.

---

### ✅ Lo que deberías llevarte de esta lección

- La web es un diálogo **petición → respuesta** (HTTP: GET pide, POST envía).
- **MVC** separa: Modelo (datos), Vista (pantalla), Controlador (coordinador).
- Toda petición sigue el ciclo **URL → Controlador → BD → Vista → HTML**.
- Cada carpeta del proyecto tiene un propósito claro, y las **convenciones de
  nombres** conectan las piezas automáticamente.

➡️ Siguiente: [02 · Entity Framework Core](02-entity-framework.md)
