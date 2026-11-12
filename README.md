# ConectaLibros

Sistema web para gestionar autores, librerías, libros y las solicitudes de libros que realizan las librerías.

Proyecto obligatorio de la materia **Aplicaciones Web en C#** de la carrera Analista de Sistemas (BIOS). **Estado: en desarrollo.**

## Tecnologías

- C# sobre .NET Framework 4.5.2
- ASP.NET MVC
- ADO.NET (acceso a datos conectado)
- SQL Server y Transact-SQL (stored procedures)
- HTML5, CSS3 y JavaScript

## Arquitectura

La solución sigue una arquitectura en 3 capas, separada en bibliotecas de clases:

| Carpeta | Capa | Responsabilidad |
|---|---|---|
| `Sitio` | Presentación | Sitio ASP.NET MVC: controladores, vistas y menú principal |
| `Logica` | Lógica de negocio | Reglas del dominio, validaciones y manejo de excepciones |
| `Persistencia` | Acceso a datos | Comunicación con SQL Server mediante ADO.NET y stored procedures |
| `EntidadesCompartidas` | Entidades | Clases del dominio compartidas entre capas, con validación de sus datos |
| `Base de datos` | Datos | Script de creación del esquema, stored procedures y datos de prueba |

Todo el acceso a la base de datos se hace exclusivamente a través de stored procedures. Los errores entre componentes se comunican mediante excepciones.

## Funcionalidades

- Mantenimiento (alta, baja, modificación y consulta) de **Autores**, con la restricción de no eliminar autores que tengan libros asociados.
- Mantenimiento de **Librerías**, con la restricción de no eliminar librerías que tengan solicitudes asociadas, y la lista de solicitudes de cada librería.
- Alta de **Libros**, indicando editorial y autor.
- Alta de **Solicitudes** de libros, validando que el libro tenga stock disponible y que la fecha de entrega propuesta sea al menos 24 horas posterior a la solicitud.
- **Listado de Editoriales** ordenado por nombre, con los libros publicados y las solicitudes asociadas a cada una.

## Cómo ejecutarlo

1. Abrir SQL Server Management Studio y ejecutar el script que está en la carpeta `Base de datos`. Crea la base de datos, las tablas, los stored procedures y carga datos de prueba.
2. Abrir la solución `ConectaLibrosAW` con Visual Studio.
3. En el `Web.config` del proyecto `Sitio`, ajustar la cadena de conexión para que apunte a tu instancia de SQL Server.
4. Establecer `Sitio` como proyecto de inicio y ejecutar (F5).

## Autora

Natasha Miquele de Melo
[GitHub](https://github.com/natymiquele08)
