# asp.net8_entity_practice

Práctica complementaria para comprender cómo se utiliza **ASP.NET Core 8** y su
integración con **Entity Framework Core**.

Este repositorio es un **curso práctico desde cero**, construido alrededor de un
proyecto real: un sistema de **RH / Capacitación** para una planta (áreas,
puestos, trabajadores, cursos y las relaciones entre ellos).

---

## 🚀 Empieza aquí

1. Lee el **[mapa del curso](docs/00-bienvenida-y-mapa.md)**.
2. Sigue las guías en orden (están en la carpeta [`docs/`](docs/)).
3. Ejecuta el proyecto y experimenta.

### Guías disponibles

| # | Guía |
|---|------|
| 00 | [Bienvenida y mapa del curso](docs/00-bienvenida-y-mapa.md) |
| 01 | [Fundamentos de ASP.NET Core MVC](docs/01-fundamentos-aspnet-mvc.md) |
| 02 | [Entity Framework Core](docs/02-entity-framework.md) |
| 03 | [Módulo "Áreas", paso a paso](docs/03-modulo-areas-paso-a-paso.md) |
| 04 | [Cómo ejecutar el proyecto](docs/04-como-ejecutar.md) |

---

## ⚡ Arranque rápido

```bash
# 1) Crear la base de datos local (SQLite) a partir de las migraciones
dotnet ef database update

# 2) Levantar la aplicación
dotnet run

# 3) Abrir en el navegador la URL que aparece (p. ej. http://localhost:5xxx)
#    y entrar al módulo "Áreas".
```

¿No tienes .NET 8 o `dotnet-ef`? Míralo en la
[guía 04](docs/04-como-ejecutar.md).

---

## 🧱 Estado del proyecto

**Construido:** proyecto MVC + EF Core (SQLite) + módulo **Áreas** con CRUD
completo (crear, listar, ver, editar, borrar).

**Siguiente:** Puestos (relación uno-a-muchos), Trabajadores (perfil por
nómina), Cursos (relación muchos-a-muchos) y capacitaciones por puesto.
Ver la ruta completa en el [mapa del curso](docs/00-bienvenida-y-mapa.md).

---

## 🛠️ Tecnologías

- ASP.NET Core 8 (MVC)
- Entity Framework Core 8 (enfoque *Code First*)
- SQLite (base de datos local; fácilmente cambiable a SQL Server)
- Bootstrap 5 (estilos, incluido en la plantilla)
