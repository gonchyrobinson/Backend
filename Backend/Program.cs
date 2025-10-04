using Backend.Constants;
using Backend.Contexts;
using Backend.Helpers;
using Backend.Interfaces.Repositories;
using Backend.Interfaces.Services;
using Backend.Mappings;
using Backend.Middleware;
using Backend.Reports;
using Backend.Reports.Contrato_Pasantia_Estudiante;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Threading.RateLimiting; // <= .NET 8 Rate Limiter

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Controllers (sin filtros globales - utilizando middleware)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

// === Entity Framework (sin AutoDetect) ===
var cs = builder.Configuration.GetConnectionString("DefaultConnection");
// Asegurate de ajustar la versión si cambia tu servidor
var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(cs, serverVersion));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(AppConstants.CorsPolicyName, policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:3001",
                "https://localhost:3001",
                "http://localhost:3002",
                "https://localhost:3002",
                "http://localhost:3003",
                "https://localhost:3003",
                "http://localhost:3004",
                "https://localhost:3004",
                "http://localhost:3005",
                "https://localhost:3005",
                "http://localhost:3006",
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
                "https://127.0.0.1:4173",
                "https://gestion-pasantias-facet-g9ascqg9ckd5eggd.centralus-01.azurewebsites.net",
                "https://gestionpasantias-cchah8fwd2bqcye0.centralus-01.azurewebsites.net"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowedToAllowWildcardSubdomains();
    });

    options.AddPolicy("ProductionCors", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "https://localhost:3000",
                "http://localhost:3001",
                "https://localhost:3001",
                "http://localhost:3002",
                "https://localhost:3002",
                "http://localhost:3003",
                "https://gestionpasantias-cchah8fwd2bqcye0.centralus-01.azurewebsites.net"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

// JWT
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

// Repositorios
builder.Services.AddScoped<IRepositorioPasantias, RepositorioPasantias>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRepositorioEstudiantes, RepositorioEstudiantes>();
builder.Services.AddScoped<IRepositorioEmpresas, RepositorioEmpresas>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IRepositorioPagos, RepositorioPagos>();
builder.Services.AddScoped<IRepositorioConvenios, RepositorioConvenios>();
builder.Services.AddScoped<IRepositorioAuditoria, RepositorioAuditoria>();

// Servicios de validación
builder.Services.AddScoped<EstudianteValidationService>();
builder.Services.AddScoped<EmpresaValidationService>();
builder.Services.AddScoped<ConvenioValidationService>();
builder.Services.AddScoped<PasantiaValidationService>();
builder.Services.AddScoped<PagoValidationService>();

// Servicios principales
builder.Services.AddScoped<IServicioPasantias, ServicioPasantias>();
builder.Services.AddScoped<IServicioEmpresas, ServicioEmpresas>();
builder.Services.AddScoped<IServicioEstudiantes, ServicioEstudiantes>();
builder.Services.AddScoped<IServicioPagos, ServicioPagos>();
builder.Services.AddScoped<IServicioConvenios, ServicioConvenios>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IServicioAuditoria, ServicioAuditoria>();
builder.Services.AddScoped<IBackupService, BackupService>();

// Reporte Contrato Pasantía Estudiante
builder.Services.AddScoped<IReporteDataAggregator<int, ReportData>, ReportAggregator>();
builder.Services.AddScoped<IReporteRenderer<ReportData>, ContratoPasantiaEstudianteReportRenderer>();
builder.Services.AddScoped<ContratoPasantiaEstudianteReportService>();

// Reporte Extension Seguro
builder.Services.AddScoped<IReporteRenderer<ReportData>, Backend.Reports.ExtensionSeguro.ExtensionSeguroReportRenderer>();
builder.Services.AddScoped<Backend.Reports.ExtensionSeguro.ExtensionSeguroReportService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Backend API",
        Version = "v1",
        Description = "API para sistema de gestión de pasantías"
    });

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

// === Rate Limiting nativo (.NET 8) ===
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // 60 req por minuto global
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: "global",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 60,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
});

var app = builder.Build();

// Security headers
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    await next();
});

// HSTS solo en producción
app.UseHsts();

app.UseMiddleware<ExceptionMiddleware>();

// Swagger en desarrollo y staging
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Staging") || app.Environment.IsEnvironment("Production"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend API v1");
        c.RoutePrefix = "swagger";
    });
}

// CORS
if (app.Environment.IsDevelopment())
{
    app.UseCors(AppConstants.CorsPolicyName);
}
else
{
    app.UseCors("ProductionCors");
}

// HTTPS
app.UseHttpsRedirection();

// Rate Limiter nativo
app.UseRateLimiter();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapControllers();

// Inicialización DB segura (sin frenar la app si falla)
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (context.Database.CanConnect())
    {
        Log.Information("Conexión a la base de datos establecida correctamente");
        context.Database.EnsureCreated();
        Log.Information("Base de datos inicializada correctamente");
    }
    else
    {
        Log.Error("No se pudo conectar a la base de datos");
    }
}
catch (Exception ex)
{
    Log.Error(ex, "Error al inicializar la base de datos: {Message}", ex.Message);
}

app.Run();
