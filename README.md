# Backend API - Sistema de Gestión de Pasantías

## 📁 Estructura del Proyecto

```
Backend/
├── Backend/                    # Código fuente de la aplicación
│   ├── Controllers/           # Controladores de la API
│   ├── Models/               # Modelos de datos
│   ├── Services/             # Lógica de negocio
│   ├── Repositories/         # Acceso a datos
│   ├── DTOs/                # Objetos de transferencia de datos
│   ├── Interfaces/           # Contratos de servicios
│   ├── Contexts/            # Contexto de Entity Framework
│   ├── Mappings/            # Configuración de AutoMapper
│   ├── Exceptions/           # Excepciones personalizadas
│   └── Constants/            # Constantes de la aplicación
├── Backend.Tests/            # Pruebas unitarias
├── docker/                   # Configuración de Docker
│   ├── docker-compose.dev.yml
│   ├── docker-compose.prod.yml
│   ├── Dockerfile
│   └── .dockerignore
├── database/                 # Configuración de base de datos
│   ├── database.env
│   ├── mysql_conf/
│   └── scripts/
├── logs/                     # Archivos de logs
├── backups/                  # Respaldos de base de datos
└── manage.ps1               # Script de gestión principal
```

## 🚀 Inicio Rápido

### Prerrequisitos
- .NET 8.0 SDK
- Docker Desktop
- PowerShell (Windows)

### Comandos Disponibles

```powershell
# Iniciar entorno completo (Docker)
.\manage.ps1 start

# Ejecutar localmente
.\manage.ps1 local

# Configurar base de datos
.\manage.ps1 setup

# Ver logs
.\manage.ps1 logs

# Ver estado
.\manage.ps1 status

# Detener servicios
.\manage.ps1 stop

# Ayuda
.\manage.ps1 help
```

## 🐳 Docker

Los archivos de Docker están organizados en la carpeta `docker/`:

- `docker-compose.dev.yml` - Entorno de desarrollo
- `docker-compose.prod.yml` - Entorno de producción
- `Dockerfile` - Imagen de la aplicación
- `.dockerignore` - Archivos a ignorar

## 🗄️ Base de Datos

La configuración de la base de datos está en la carpeta `database/`:

- `database.env` - Variables de entorno
- `mysql_conf/` - Configuración de MySQL
- `scripts/` - Scripts de inicialización

### Estructura de la Base de Datos

**Base de datos**: `pasantias_db`

**Tabla principal**: `Estudiantes`
- `Id` - Identificador único
- `Nombre` - Nombre del estudiante
- `Email` - Correo electrónico
- `Carrera` - Carrera universitaria
- `FechaCreacion` - Fecha de registro

## 📝 Desarrollo

### Estructura Simple y Funcional

El proyecto sigue principios de simplicidad:

- **Controladores**: Manejan las peticiones HTTP
- **Servicios**: Contienen la lógica de negocio
- **Repositorios**: Acceden a los datos
- **DTOs**: Definen la estructura de datos de entrada/salida

### Ejemplo de Uso

```csharp
// Controlador simple
[ApiController]
[Route("api/[controller]")]
public class StudentsController : BaseController
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
    {
        var students = await _studentService.GetAllAsync();
        return Ok(students);
    }
}
```

## 🔧 Configuración

### Variables de Entorno

Copiar `env.example` a `.env` y configurar:

```env
DB_CONNECTION_STRING=Server=localhost;Database=pasantias_db;Uid=appuser;Pwd=TuPasswordSeguro123!;
```

### Base de Datos

La base de datos se configura automáticamente al ejecutar:

```powershell
.\manage.ps1 setup
```

## 🧪 Pruebas

```powershell
cd Backend.Tests
dotnet test
```

## 📊 Monitoreo

- **Logs**: `.\manage.ps1 logs`
- **Estado**: `.\manage.ps1 status`
- **Swagger**: http://localhost:5000/swagger

## 🔄 Flujo de Trabajo

1. **Desarrollo**: `.\manage.ps1 local`
2. **Pruebas**: `.\manage.ps1 start`
3. **Producción**: `.\manage.ps1 docker`

## 📚 Tecnologías

- **.NET 8.0** - Framework de desarrollo
- **Entity Framework Core** - ORM
- **AutoMapper** - Mapeo de objetos
- **MySQL** - Base de datos
- **Docker** - Contenedores
- **Swagger** - Documentación de API