using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Serilog;
using Backend.Contexts;
using Backend.Interfaces;
using Backend.Repositories;
using Backend.Services;
using Backend.Mappings;
using Backend.Constants;
using Backend.Middleware;
using AspNetCoreRateLimit;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Agregar servicios al contenedor
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
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
        policy.WithOrigins(
                "http://localhost:3000", 
                "https://localhost:3000",
                "http://localhost:5173",
                "https://localhost:5173",
                "http://localhost:4173",
                "https://localhost:4173",
                "http://localhost:5000",
                "https://localhost:5000",
                "http://127.0.0.1:3000",
                "https://127.0.0.1:3000",
                "http://127.0.0.1:5173",
                "https://127.0.0.1:5173",
                "http://127.0.0.1:4173",
                "https://127.0.0.1:4173"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
    
    // Configuración para producción (Azure App Services)
    options.AddPolicy("ProductionCors", policy =>
    {
        policy.WithOrigins(
                "https://gestion-pasantias-facet-g9ascqg9ckd5eggd.centralus-01.azurewebsites.net"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configurar JWT Authentication
var jwtSecret = builder.Configuration["Jwt:SecretKey"];
if (string.IsNullOrWhiteSpace(jwtSecret) || Encoding.UTF8.GetByteCount(jwtSecret) < 32)
{
    throw new InvalidOperationException(
        "JWT SecretKey is missing or too short. Configure 'Jwt:SecretKey' (or env var 'Jwt__SecretKey') with at least 32 characters.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Registrar repositorios
builder.Services.AddScoped<IRepositorioPasantias, RepositorioPasantias>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRepositorioEstudiantes, RepositorioEstudiantes>();
builder.Services.AddScoped<IRepositorioEmpresas, RepositorioEmpresas>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IRepositorioPagos, RepositorioPagos>();
builder.Services.AddScoped<IRepositorioConvenios, RepositorioConvenios>();
builder.Services.AddScoped<IRepositorioAuditoria, RepositorioAuditoria>();

// Registrar servicios
builder.Services.AddScoped<ServicioPasantias>();
builder.Services.AddScoped<ServicioEmpresas>();
builder.Services.AddScoped<ServicioEstudiantes>();
builder.Services.AddScoped<ServicioPagos>();
builder.Services.AddScoped<ServicioConvenios>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ServicioAuditoria>();

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
    
    // Configurar autenticación JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();


// Middleware de headers de seguridad (CSP, X-Frame-Options, etc.)
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    await next();
});

// HSTS solo en producción
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

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

// Usar CORS antes de otros middleware
if (app.Environment.IsDevelopment())
{
    app.UseCors(AppConstants.CorsPolicyName);
}
else
{
    app.UseCors("ProductionCors");

    // Middleware de Rate Limiting
    app.UseIpRateLimiting();
}

app.UseHttpsRedirection();

app.UseAuthentication();
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
