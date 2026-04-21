Prueba Técnica - Backend .NET 8 (Rick & Morty API)

Descripción

Este proyecto consiste en el desarrollo de un servicio backend en .NET 8 Web API que permite:

Importar personajes desde la API pública de Rick and Morty
Gestionar solicitudes de contratación asociadas a dichos personajes
Exponer una API REST documentada
Aplicar buenas prácticas de arquitectura, validación, manejo de errores y contenerización


Arquitectura

Se implementó una Clean Architecture simplificada, separando responsabilidades en:

Domain → Entidades y enums
Application → Casos de uso, DTOs, interfaces, validaciones
Infrastructure → Acceso a datos (EF Core) y servicios externos
API → Controllers, middleware, configuración
Principios aplicados:
Separación de responsabilidades
Dependency Inversion Principle (DIP)
Uso de interfaces para repositorios y servicios
Controllers delgados (sin lógica de negocio)

Tecnologías utilizadas
.NET 8 Web API
Entity Framework Core (Code First + Migrations)
SQLite
FluentValidation
HttpClient (consumo API externa)
Swagger / OpenAPI
Docker & Docker Compose


API Externa

Se consume la API pública:

https://rickandmortyapi.com

Se implementó:

Paginación completa
Manejo de rate limiting (429)
Reintentos con backoff

Funcionalidades
Personajes
POST /api/personajes/import → Importar personajes
GET /api/personajes → Listado paginado con filtros (nombre, estado)
GET /api/personajes/{id} → Detalle de personaje

Solicitudes
POST /api/solicitudes → Crear solicitud
GET /api/solicitudes → Listado con filtros (estado, solicitante)
GET /api/solicitudes/{id} → Detalle de solicitud
PATCH /api/solicitudes/{id}/status → Actualizar estado

Estados:

Pending
InProgress
Approved
Rejected

Reportes
GET /api/reportes/solicitudesresumen

Incluye:

Totales por estado
Personaje más solicitado

Health Check
GET /health

Endpoint requerido para monitoreo (Azure compatible)


Validaciones

Se implementaron validaciones con FluentValidation:

Campos obligatorios
Fechas válidas
Reglas de negocio


Manejo de errores

Se implementó middleware global que:

Captura excepciones
Retorna respuestas estructuradas
Incluye traceId para debugging


Base de datos
SQLite (archivo local)
Ubicación: /Data/app.db
Configurada mediante connection string (no hardcodeada)


Docker
Ejecutar proyecto

Desde la raíz:

docker compose up --build

Acceso
Swagger: http://localhost:5000/swagger
Health: http://localhost:5000/health


Configuración

La cadena de conexión se maneja desde:

"ConnectionStrings": {
  "DefaultConnection": "Data Source=Data/app.db"
}


Preparación para Azure

El proyecto está preparado para despliegue en Azure:

Uso de variables de entorno
Endpoint /health
Configuración desacoplada
Contenerización lista


Decisiones técnicas
Uso de Clean Architecture para escalabilidad y mantenibilidad
Repository Pattern para desacoplar EF Core
DTOs para evitar exponer entidades
Manejo de rate limiting en consumo externo
Paginación en endpoints críticos
Validación centralizada con FluentValidation


Cómo probar
Levantar el proyecto con Docker
Ir a Swagger
Ejecutar:
Importación de personajes
Crear solicitudes
Consultar endpoints
Probar filtros y reportes


Migraciones de Base de Datos

El proyecto utiliza Entity Framework Core (Code First) para la creación y actualización de la base de datos.


Crear la base de datos (primera vez)
dotnet ef migrations add InitialCreate
dotnet ef database update


Importante
Ejecutar los comandos desde la carpeta donde se encuentra el archivo .csproj
La base de datos se genera automáticamente en:
/Data/app.db


Notas
Archivos SQLite (.db, .shm, .wal) están excluidos del repositorio
La base de datos se genera automáticamente con migraciones
No es necesario crear la base de datos manualmente
Si la base de datos se elimina, puede recrearse ejecutando nuevamente los comandos anteriores