# 00 · Bienvenida y mapa del curso

¡Hola! Este repositorio es tu **curso práctico** de ASP.NET Core 8 + Entity
Framework Core, construido alrededor de un proyecto real: un sistema de
**RH / Capacitación** para una planta.

Partimos de que **no sabes nada** todavía, y eso está perfecto. Iremos paso a
paso, y cada pieza de código está comentada en español explicando el *por qué*.

---

## ¿Qué vamos a construir? (el proyecto)

Un sistema donde el equipo de **Recursos Humanos / Capacitación** pueda:

- Gestionar las **áreas** de la planta (Producción, Almacén, Calidad...).
- Gestionar los **puestos** de trabajo.
- Registrar a los **trabajadores** y ver su perfil por su **número de nómina**.
- Administrar los **cursos** (capacitaciones), cada uno ligado a un área.
- Saber **qué capacitaciones necesita cada trabajador según su puesto**.
- Llevar la **planeación** mensual y anual de cursos.

No haremos todo de golpe. Lo construimos **módulo por módulo**, y en cada uno
aprendes un concepto nuevo.

---

## El modelo de datos al que llegaremos

Estas son las "tablas" (entidades) y cómo se relacionan. No te preocupes si aún
no entiendes las flechas; las iremos viendo una por una.

```
  Area 1 ────< Curso            (un área tiene muchos cursos)
  Area 1 ────< Puesto           (un área tiene muchos puestos)   [según diseñemos]
  Puesto 1 ──< Trabajador       (un puesto lo tienen muchos trabajadores)
  Puesto >──< Curso             (un puesto requiere varios cursos, y un curso
                                 aplica a varios puestos = muchos-a-muchos)
  Trabajador >──< Curso         (un trabajador toma varios cursos, con fecha y
             (a través de)       calificación = tabla intermedia "Capacitacion")
```

- `1 ────<`  se lee **"uno a muchos"**.
- `>──<`     se lee **"muchos a muchos"**.

---

## Ruta de aprendizaje (el orden de las lecciones)

| # | Lección | Qué aprendes | Estado |
|---|---------|--------------|--------|
| 01 | [Fundamentos de ASP.NET Core MVC](01-fundamentos-aspnet-mvc.md) | Qué es la web, MVC, cómo viaja una petición, estructura del proyecto | ✅ |
| 02 | [Entity Framework Core](02-entity-framework.md) | ORM, DbContext, entidades, migraciones, Code First | ✅ |
| 03 | [Módulo Áreas, paso a paso](03-modulo-areas-paso-a-paso.md) | Tu primer CRUD completo, archivo por archivo | ✅ |
| 04 | [Cómo ejecutar el proyecto](04-como-ejecutar.md) | Comandos del día a día para correr y trabajar | ✅ |
| 05 | Módulo Puestos + relación con Áreas | Relaciones **uno a muchos**, claves foráneas | 🔜 |
| 06 | Módulo Trabajadores + perfil por nómina | Búsquedas, índices únicos, `Include` | 🔜 |
| 07 | Cursos + relación **muchos a muchos** con Puestos | El corazón de tu proyecto | 🔜 |
| 08 | Capacitaciones del trabajador (avances, calificaciones) | Tabla intermedia con datos extra | 🔜 |
| 09 | Login y permisos (Identity) | Quién puede entrar y qué puede hacer | 🔜 |

✅ = listo para leer · 🔜 = lo construimos en las próximas sesiones

---

## Cómo usar este curso

1. **Lee las guías en orden** (empieza por la 01).
2. **Abre el código** que cada guía menciona y léelo con sus comentarios.
3. **Ejecuta el proyecto** (guía 04) y juega con él en el navegador.
4. **Rompe cosas a propósito**: cambia un texto, quita un `[Required]`, mira qué
   pasa. Equivocarse es la forma más rápida de aprender.
5. Cuando estés listo, avísame y **construimos el siguiente módulo juntos**.

> Consejo: no intentes memorizar. Entiende el *flujo* (petición → controlador →
> base de datos → vista). Los detalles se te quedan solos con la práctica.
