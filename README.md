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

## 🔐 Autenticación JWT

El sistema implementa autenticación JWT para la seguridad de la API.

### Configuración JWT

Asegúrate de que el `appsettings.json` tenga la configuración JWT:

```json
{
  "Jwt": {
    "SecretKey": "your-super-secret-key-with-at-least-32-characters-for-jwt-signing",
    "Issuer": "BackendAPI",
    "Audience": "ReactApp",
    "AccessTokenExpirationMinutes": 15
  }
}
```

### Endpoints de Autenticación

#### POST /api/v1/authn/register
Registra un nuevo usuario.

**Request:**
```json
{
  "username": "nuevo_usuario",
  "email": "usuario@ejemplo.com",
  "password": "contraseña123"
}
```

#### POST /api/v1/authn/login
Inicia sesión con credenciales de usuario.

**Request:**
```json
{
  "username": "admin",
  "password": "admin123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "admin",
    "email": "admin@pasantias.com",
    "role": "admin"
  }
}
```



#### GET /api/v1/authn/session
Obtiene información de la sesión actual.

**Headers:** `Authorization: Bearer {token}`

#### GET /api/v1/authn/validate
Valida si el token actual es válido.

**Headers:** `Authorization: Bearer {token}`


### Protección de Endpoints

Para proteger un endpoint, usa el atributo `[Authorize]`:

```csharp
[HttpGet("protected")]
[Authorize]
public IActionResult ProtectedEndpoint()
{
    return Ok("Este endpoint está protegido");
}
```

Para requerir un rol específico:

```csharp
[HttpGet("admin-only")]
[Authorize(Roles = "admin")]
public IActionResult AdminOnly()
{
    return Ok("Solo para administradores");
}
```

### Flujo de Autenticación

1. **Login:** Usuario envía credenciales → Backend valida → Retorna access token
2. **Acceso:** Cliente incluye access token en header `Authorization: Bearer {token}`

### Seguridad

- **Access Token:** Expira en 15 minutos
- **Hash:** Las contraseñas se hashean con SHA256

### CORS Configuration

El backend está configurado para aceptar peticiones desde:
- http://localhost:3000
- http://localhost:5173
- http://localhost:4173
- http://127.0.0.1:3000
- http://127.0.0.1:5173
- http://127.0.0.1:4173

### Testing con Swagger

1. Ve a `/swagger` en tu navegador
2. Haz clic en "Authorize" en la parte superior
3. Ingresa tu token: `Bearer {tu_token}`
4. Ahora puedes probar los endpoints protegidos

### Ejemplo de Uso con Frontend

```javascript
// Login
const response = await fetch('/api/v1/authn/login', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ username: 'admin', password: 'admin123' })
});

const { token } = await response.json();

// Usar token en requests
const data = await fetch('/api/v1/authn/session', {
  headers: { 'Authorization': `Bearer ${token}` }
});
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
- **JWT** - Autenticación y autorización