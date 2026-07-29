# Prueba Técnica - Gestión de Regiones y Comunas

## Descripción

Solución desarrollada en .NET 8 compuesta por tres proyectos:

- **PruebaTecnica.DataAccess**: acceso a datos mediante ADO.NET y procedimientos almacenados.
- **PruebaTecnica.Api**: API REST que expone los servicios de consulta y actualización.
- **PruebaTecnica.Web**: aplicación ASP.NET Core MVC que consume la API y permite administrar la información de las comunas.

## Tecnologías

- .NET 8
- ASP.NET Core MVC
- ASP.NET Core Web API
- SQL Server
- ADO.NET
- Bootstrap 5
- JavaScript (ES6)

## Arquitectura

La solución está dividida en tres capas:

- Acceso a datos
- API REST
- Aplicación MVC

La comunicación entre la aplicación MVC y la base de datos se realiza exclusivamente a través de la API.

## Base de datos

La carpeta **BaseDatos** contiene:

- Script de creación de tablas.
- Procedimientos almacenados.
- Datos de prueba.
- (Opcional) Backup de la base de datos.

## Características implementadas

- Listado de regiones.
- Consulta de una región.
- Listado de comunas por región.
- Consulta de una comuna.
- Actualización de comunas mediante MERGE.
- Integración 100% mediante procedimientos almacenados.
- Consumo de servicios REST en formato JSON.
- Manejo básico de excepciones y registro mediante ILogger.

## Decisiones de diseño

- Uso de interfaces para desacoplar los repositorios.
- SqlConnectionFactory para centralizar la creación de conexiones.
- Parseo y generación del XML encapsulados en métodos privados.
- Separación entre la API REST y la aplicación MVC.
- Consumo de la API mediante HttpClient.

## Ejecución

1. Restaurar la base de datos o ejecutar el script SQL.
2. Configurar la cadena de conexión en `appsettings.json` del proyecto API.
3. Ejecutar primero **PruebaTecnica.Api**.
4. Ejecutar **PruebaTecnica.Web**.

## Autor

Jorge Vargas