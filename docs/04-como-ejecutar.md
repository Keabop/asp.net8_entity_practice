# 04 · Cómo ejecutar el proyecto

Guía práctica de los comandos que usarás a diario. Todos se ejecutan **desde la
carpeta del proyecto** (donde está `CapacitacionRH.csproj`).

---

## 0. Requisito: tener instalado el SDK de .NET 8

Comprueba si ya lo tienes:

```bash
dotnet --version      # debe mostrar algo como 8.0.xxx
```

Si no lo tienes, descárgalo gratis de: https://dotnet.microsoft.com/download/dotnet/8.0
(elige **SDK**, no solo "Runtime"). En Windows, el instalador lo deja listo.

También necesitas la herramienta de migraciones (una sola vez):

```bash
dotnet tool install --global dotnet-ef --version 8.0.11
```

---

## 1. Preparar la base de datos (la primera vez)

El archivo de base de datos (`CapacitacionRH.db`) **no** está en el repositorio
(lo ignora `.gitignore`, porque cada quien lo genera). Créalo con:

```bash
dotnet ef database update
```

Esto lee las migraciones de la carpeta `Migrations/` y construye la BD con sus
tablas. Cuando termine, verás aparecer el archivo `CapacitacionRH.db`.

---

## 2. Ejecutar la aplicación

```bash
dotnet run
```

Verás en la consola algo como:

```
Now listening on: http://localhost:5xxx
```

Abre esa dirección en tu navegador. Prueba entrar a **/Areas** (o usa el menú
"Áreas" de arriba). Crea, edita y borra áreas para ver el CRUD en acción.

Para **detener** la app: pulsa `Ctrl + C` en la consola.

> 💡 Durante el desarrollo puedes usar `dotnet watch run`: recompila y recarga
> solo al guardar cambios en el código. Muy cómodo.

---

## 3. Cuando agregues o cambies una entidad (el ciclo EF)

Cada vez que modifiques un modelo o crees uno nuevo, repites:

```bash
dotnet ef migrations add DescripcionDelCambio   # genera la migración
dotnet ef database update                       # la aplica a la BD
```

Ejemplos de nombres descriptivos: `AgregarPuestos`, `AgregarNominaATrabajador`,
`RelacionCursoArea`.

---

## Chuleta de comandos (para tener a mano)

| Necesito... | Comando |
|-------------|---------|
| Compilar y ver si hay errores | `dotnet build` |
| Ejecutar la app | `dotnet run` |
| Ejecutar y recargar al guardar | `dotnet watch run` |
| Crear una migración | `dotnet ef migrations add <Nombre>` |
| Aplicar migraciones a la BD | `dotnet ef database update` |
| Deshacer la última migración (si aún NO la aplicaste) | `dotnet ef migrations remove` |
| Volver la BD a una migración anterior | `dotnet ef database update <NombreMigracion>` |
| Ver las migraciones existentes | `dotnet ef migrations list` |
| Instalar un paquete NuGet | `dotnet add package <Nombre>` |

---

## Problemas comunes (y su solución)

**"Unable to create a 'DbContext'..." al correr un comando `dotnet ef`.**
→ Asegúrate de estar en la carpeta del `.csproj` y de que el proyecto compila
(`dotnet build`).

**Cambié un modelo pero no veo el cambio en la BD.**
→ Te faltó el ciclo: `dotnet ef migrations add ...` **y** `dotnet ef database update`.

**Quiero empezar la base de datos desde cero.**
→ Borra el archivo `CapacitacionRH.db` y vuelve a correr `dotnet ef database update`.
(Solo en desarrollo: en producción NUNCA se borra la BD, se migra con cuidado.)

**El puerto está ocupado.**
→ Especifica otro: `dotnet run --urls http://localhost:5080`.

---

### ✅ Lo que deberías llevarte de esta lección

- `dotnet run` levanta la app; `Ctrl+C` la detiene.
- El ciclo de EF ante cualquier cambio de datos: **`migrations add`** →
  **`database update`**.
- Guarda la chuleta de comandos: son los que usarás cada día.

↩️ Volver al [mapa del curso](00-bienvenida-y-mapa.md)
