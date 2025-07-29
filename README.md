# Backend - Sistema de Gestión de Pasantías

Backend ASP.NET Core .NET 8 para sistema de gestión de pasantías con arquitectura Clean Architecture simplificada.

## Tecnologías Utilizadas

- **ASP.NET Core .NET 8** - Framework web
- **Entity Framework Core 8.0** - ORM para acceso a datos
- **MySQL** - Base de datos
- **AutoMapper** - Mapeo de objetos
- **Swagger/OpenAPI** - Documentación de API
- **Serilog** - Logging
- **xUnit** - Testing framework

## Estructura del Proyecto

```
Backend/
├── Controllers/          # Controladores REST API
├── Models/              # Entidades de Entity Framework
├── DTOs/               # Data Transfer Objects
├── Services/           # Lógica de negocio
├── Repositories/       # Acceso a datos
├── Interfaces/         # Interfaces para servicios y repositorios
├── Mappings/          # Configuración de AutoMapper
├── Constants/         # Constantes del sistema
├── Exceptions/        # Excepciones personalizadas
├── Data/              # Contexto de Entity Framework
└── Properties/        # Configuración de launch settings
```

## Configuración

### 1. Base de Datos

Configura la cadena de conexión en `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=internships_db;User=root;Password=password;"
}
```

### 2. Instalación de Dependencias

```bash
dotnet restore
```

### 3. Migraciones (cuando se agreguen nuevas entidades)

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Ejecución

### Desarrollo

```bash
dotnet run
```

La API estará disponible en:
- **API**: http://localhost:5000/api
- **Swagger**: http://localhost:5000/swagger

### Tests

```bash
dotnet test
```

## Endpoints Disponibles

### Students
- `GET /api/students` - Obtener todos los estudiantes
- `GET /api/students/{id}` - Obtener estudiante por ID
- `POST /api/students` - Crear nuevo estudiante
- `PUT /api/students/{id}` - Actualizar estudiante
- `DELETE /api/students/{id}` - Eliminar estudiante

## Características Implementadas

✅ API responde en http://localhost:5000/api  
✅ Swagger disponible en /swagger  
✅ CORS configurado para frontend React  
✅ CRUD completo para entidad Student  
✅ AutoMapper configurado  
✅ Inyección de dependencias con alcance Scoped  
✅ Tests unitarios para servicios  
✅ Entity Framework con configuración MySQL  
✅ Estructura de carpetas según especificación  
✅ Properties/launchSettings.json configurado  
✅ .NET 8 implementado  

## Próximos Pasos

Para agregar nuevas entidades:

1. Crear el modelo en `Models/`
2. Crear el DTO en `DTOs/`
3. Crear el repositorio específico en `Repositories/`
4. Crear el servicio específico en `Services/`
5. Crear el controlador específico en `Controllers/`
6. Configurar el mapeo en `Mappings/MappingProfile.cs`
7. Registrar servicios en `Program.cs`
8. Agregar tests unitarios

## Notas

- El proyecto usa una arquitectura simplificada con Repository Pattern
- Todos los servicios usan inyección de dependencias con alcance Scoped
- AutoMapper está configurado para mapeos automáticos
- Los tests usan Entity Framework In-Memory para aislamiento