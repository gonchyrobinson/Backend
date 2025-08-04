using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Serilog;
using Backend.Contexts;
using Backend.Interfaces;
using Backend.Repositories;
using Backend.Services;
using Backend.Mappings;
using Backend.Constants;
using Backend.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Agregar servicios al contenedor
builder.Services.AddControllers(options =>
{
    options.Filters.Add<Backend.Middleware.ApiExceptionFilter>();
});

// Configurar Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(AppConstants.CorsPolicyName, policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Registrar repositorios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRepositorioEstudiantes, RepositorioEstudiantes>();
builder.Services.AddScoped<IRepositorioEmpresas, RepositorioEmpresas>();

// Registrar servicios
builder.Services.AddScoped<ServicioEstudiantes>();
builder.Services.AddScoped<ServicioEmpresas>();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Backend API", 
        Version = "v1",
        Description = "API para sistema de gestión de pasantías"
    });
});

var app = builder.Build();

// Registrar el middleware de excepciones personalizado
app.UseMiddleware<ExceptionMiddleware>();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// Usar CORS
app.UseCors(AppConstants.CorsPolicyName);

app.UseAuthorization();

app.MapControllers();

// Crear la base de datos si no existe con manejo de errores
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // Verificar conexión
        if (context.Database.CanConnect())
        {
            Log.Information("Conexión a la base de datos establecida correctamente");
            
            // Crear la base de datos si no existe
            context.Database.EnsureCreated();
            Log.Information("Base de datos inicializada correctamente");
        }
        else
        {
            Log.Error("No se pudo conectar a la base de datos");
        }
    }
}
catch (Exception ex)
{
    Log.Error(ex, "Error al inicializar la base de datos: {Message}", ex.Message);
    // No detener la aplicación, solo registrar el error
}

app.Run();
