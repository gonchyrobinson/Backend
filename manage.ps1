# =====================================================
# Script Principal de Gestión - Backend API
# =====================================================

param(
    [Parameter(Position=0)]
    [ValidateSet("start", "stop", "local", "docker", "setup", "logs", "status", "help")]
    [string]$Action = "help"
)

function Show-Help {
    Write-Host "=== GESTOR DE BACKEND API ===" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Uso: .\manage.ps1 [comando]" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Comandos disponibles:" -ForegroundColor Green
    Write-Host "  start   - Iniciar entorno completo (Docker)" -ForegroundColor Green
    Write-Host "  stop    - Detener todos los servicios" -ForegroundColor Green
    Write-Host "  local   - Ejecutar aplicación localmente" -ForegroundColor Green
    Write-Host "  docker  - Ejecutar aplicación en Docker" -ForegroundColor Green
    Write-Host "  setup   - Configurar base de datos" -ForegroundColor Green
    Write-Host "  logs    - Mostrar logs de servicios" -ForegroundColor Green
    Write-Host "  status  - Mostrar estado de servicios" -ForegroundColor Green
    Write-Host "  help    - Mostrar esta ayuda" -ForegroundColor Green
    Write-Host ""
    Write-Host "Ejemplos:" -ForegroundColor Yellow
    Write-Host "  .\manage.ps1 start" -ForegroundColor Gray
    Write-Host "  .\manage.ps1 local" -ForegroundColor Gray
    Write-Host "  .\manage.ps1 setup" -ForegroundColor Gray
}

function Start-Services {
    Write-Host "Iniciando entorno completo..." -ForegroundColor Green
    
    # Verificar Docker
    try {
        docker version | Out-Null
        Write-Host "Docker está ejecutándose" -ForegroundColor Green
    } catch {
        Write-Host "Error: Docker no está ejecutándose. Inicia Docker Desktop primero." -ForegroundColor Red
        return
    }
    
    # Detener servicios existentes
    Write-Host "Deteniendo servicios existentes..." -ForegroundColor Yellow
    docker-compose -f docker/docker-compose.dev.yml down 2>$null
    docker-compose -f docker/docker-compose.prod.yml down 2>$null
    
    # Iniciar servicios
    Write-Host "Iniciando servicios..." -ForegroundColor Yellow
    docker-compose -f docker/docker-compose.dev.yml up --build -d
    
    # Esperar a que MySQL esté listo
    Write-Host "Esperando a que MySQL esté listo..." -ForegroundColor Yellow
    Start-Sleep -Seconds 30
    
    # Verificar estado
    Write-Host "Estado de los servicios:" -ForegroundColor Cyan
    docker-compose -f docker/docker-compose.dev.yml ps
    
    Write-Host "`nEntorno iniciado correctamente!" -ForegroundColor Green
    Write-Host "API: http://localhost:5000" -ForegroundColor Cyan
    Write-Host "Swagger: http://localhost:5000/swagger" -ForegroundColor Cyan
}

function Stop-Services {
    Write-Host "Deteniendo todos los servicios..." -ForegroundColor Yellow
    docker-compose -f docker/docker-compose.dev.yml down 2>$null
    docker-compose -f docker/docker-compose.prod.yml down 2>$null
    Write-Host "Servicios detenidos" -ForegroundColor Green
}

function Start-Local {
    Write-Host "Ejecutando aplicación localmente..." -ForegroundColor Green
    
    # Verificar puerto 5000
    $portInUse = netstat -ano | findstr ":5000"
    if ($portInUse) {
        Write-Host "Puerto 5000 está en uso. Deteniendo servicios Docker..." -ForegroundColor Yellow
        docker-compose -f docker/docker-compose.dev.yml down 2>$null
        Start-Sleep -Seconds 3
    }
    
    # Configurar base de datos
    Setup-Database
    
    # Ejecutar aplicación
    Set-Location -Path "Backend"
    dotnet restore
    dotnet run
}

function Start-Docker {
    Write-Host "Ejecutando aplicación en Docker..." -ForegroundColor Green
    Start-Services
}

function Setup-Database {
    Write-Host "Configurando base de datos..." -ForegroundColor Green
    
    # Verificar MySQL
    $mysqlRunning = docker ps --filter "name=backend-mysql-dev" --format "table {{.Names}}" | Select-String "backend-mysql-dev"
    
    if (-not $mysqlRunning) {
        Write-Host "Iniciando MySQL..." -ForegroundColor Yellow
        docker run --name backend-mysql-dev -e MYSQL_ROOT_PASSWORD=TuPasswordRoot123! -e MYSQL_DATABASE=pasantias_db -e MYSQL_USER=appuser -e MYSQL_PASSWORD=TuPasswordSeguro123! -p 3306:3306 -d mysql:8.0 --default-authentication-plugin=mysql_native_password
        Start-Sleep -Seconds 30
    }
    
    # Ejecutar script de inicialización
    Write-Host "Inicializando base de datos..." -ForegroundColor Yellow
    Get-Content database/scripts/init_db.sql | docker exec -i backend-mysql-dev mysql -u root -pTuPasswordRoot123!
    
    Write-Host "Base de datos configurada correctamente" -ForegroundColor Green
}

function Show-Logs {
    Write-Host "Logs de servicios:" -ForegroundColor Cyan
    docker-compose -f docker/docker-compose.dev.yml logs -f
}

function Show-Status {
    Write-Host "Estado de servicios:" -ForegroundColor Cyan
    docker ps --filter "name=backend"
    
    Write-Host "`nEstado de la base de datos:" -ForegroundColor Cyan
    docker exec backend-mysql-dev mysql -u root -pTuPasswordRoot123! -e "USE pasantias_db; SELECT COUNT(*) as TotalEstudiantes FROM Estudiantes;" 2>$null
}

# Ejecutar comando según parámetro
switch ($Action) {
    "start" { Start-Services }
    "stop" { Stop-Services }
    "local" { Start-Local }
    "docker" { Start-Docker }
    "setup" { Setup-Database }
    "logs" { Show-Logs }
    "status" { Show-Status }
    "help" { Show-Help }
    default { Show-Help }
}