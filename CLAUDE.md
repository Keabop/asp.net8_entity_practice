# CLAUDE.md — Contexto del proyecto (léeme primero)

> Este archivo le da contexto a Claude Code sobre este repositorio. Si eres una
> nueva sesión de Claude, **lee esto completo antes de responder**.

## Qué es este repo

Es un **curso práctico de ASP.NET Core 8 (MVC) + Entity Framework Core**,
construido alrededor de un proyecto real: un **sistema de RH / Capacitación**
para una planta (áreas, puestos, trabajadores, cursos/capacitaciones y sus
relaciones). El objetivo NO es solo tener la app, sino que el usuario **aprenda**.

## Sobre el usuario y CÓMO enseñarle (muy importante)

- Parte **desde cero**: no sabía nada de ASP.NET ni de Entity Framework. Va a
  usar esto para un proyecto grande en una empresa, así que necesita **entender
  de fondo**, no solo copiar.
- **Habla español** — respóndele siempre en español, con lenguaje sencillo y
  analogías. Explica el *por qué*, no solo el *cómo*. Evita jerga sin traducir.
- **Quiere practicar escribiendo él mismo el código.** A partir del módulo de
  Puestos, el enfoque es: Claude **explica el concepto y guía**, el **usuario
  escribe el código**, y Claude **revisa, corrige y responde dudas**. NO le
  entregues módulos completos ya hechos; dale ejercicios y acompáñalo.
- Trabaja en **Windows con Visual Studio 2022**. Para migraciones usa la
  **Consola del Administrador de paquetes** (`Add-Migration`, `Update-Database`).
- Todo el código está **comentado en español** con fines didácticos; mantén ese
  estilo en lo nuevo.

## Estado actual del curso

- ✅ **Módulo Áreas**: CRUD completo (Crear/Leer/Editar/Borrar). Sirvió para
  aprender el flujo MVC + EF de punta a punta. Ver `Controllers/AreasController.cs`,
  `Models/Area.cs`, `Views/Areas/`.
- 🔜 **Módulo Puestos (EN CURSO)**: introduce la primera **relación uno-a-muchos**
  (un Área tiene muchos Puestos; un Puesto pertenece a un Área). El usuario está
  haciéndolo como ejercicio guiado:
  - Tarea 1 (en progreso): crear `Models/Puesto.cs` con `AreaId` (llave foránea)
    y `Area` (navegación); agregar `ICollection<Puesto> Puestos` en `Area.cs`;
    agregar `DbSet<Puesto>` al contexto; y correr `Add-Migration AgregarPuestos`
    + `Update-Database`.
  - Tarea 2 (siguiente): controlador y vistas de Puestos, adaptando el patrón de
    Áreas, agregando un `<select>` (menú desplegable) para elegir el Área.

## Roadmap (siguiente a construir, en orden)

1. Puestos + relación uno-a-muchos con Áreas ← aquí vamos
2. Trabajadores + perfil por **número de nómina** (índice único, `Include`)
3. Cursos (capacitaciones) + relación **muchos-a-muchos** con Puestos
4. Capacitaciones del trabajador (avances, calificaciones, tabla intermedia)
5. Login y permisos (ASP.NET Core Identity)

Las guías escritas del curso están en `docs/` (léelas para más contexto).

## Arquitectura / stack

- ASP.NET Core 8, patrón **MVC** (Controladores + Vistas Razor `.cshtml`).
- **Entity Framework Core 8**, enfoque **Code First**. Proveedor: **SQLite**
  (archivo `CapacitacionRH.db`, ignorado por git; se cambia fácil a SQL Server).
- `Program.cs` aplica migraciones al arrancar (`db.Database.Migrate()`) para que
  la BD se cree sola en desarrollo.
- Bootstrap 5 para estilos (incluido en la plantilla).

## Comandos útiles

```bash
dotnet build                 # compilar
dotnet run                   # ejecutar (o F5 en Visual Studio)
dotnet ef migrations add X   # crear migración (o Add-Migration X en la consola de VS)
dotnet ef database update    # aplicar migraciones (o Update-Database en la consola de VS)
```

## Convenciones

- Nombres de dominio en **español** (Area, Puesto, Trabajador, Curso).
- Un controlador por módulo; vistas en `Views/<Controlador>/`.
- Mantener comentarios explicativos en español en el código nuevo.
