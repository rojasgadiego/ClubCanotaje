using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Helper;
using ClubCanotajeAPI.Helper.Interface;
using ClubCanotajeAPI.Middleware;
using ClubCanotajeAPI.Repositories.CanoaRepository;
using ClubCanotajeAPI.Repositories.EventoRepository;
using ClubCanotajeAPI.Repositories.ImplementoRepository;
using ClubCanotajeAPI.Repositories.MantenedorRepository;
using ClubCanotajeAPI.Repositories.MembresiaRepository;
using ClubCanotajeAPI.Repositories.RemadorRepository;
using ClubCanotajeAPI.Repositories.SalidaRepository;
using ClubCanotajeAPI.Repositories.Usuario;
using ClubCanotajeAPI.Repositories.Verificacion;
using ClubCanotajeAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── Serilog ────────────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ── Base de datos ──────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            );
        }
    )
);

// ── Repositorios ───────────────────────────────────────────────────────────
builder.Services.AddScoped<IRemadorRepository, RemadorRepository>();
// builder.Services.AddScoped<CanoaRepository>();
builder.Services.AddScoped<ISalidaRepository, SalidaRepository>();
// builder.Services.AddScoped<ImplementoRepository>();
// builder.Services.AddScoped<MembresiaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IVerificacionRepository, VerificacionRepository>();
// builder.Services.AddScoped<EventoRepository>();
// builder.Services.AddScoped<MantenedorRepository>();

// ── Servicios ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<EmailService>();
// builder.Services.AddScoped<RemadorService>();
// builder.Services.AddScoped<CanoaService>();
builder.Services.AddScoped<ISalidaService, SalidaService>();
// builder.Services.AddScoped<ImplementoService>();
// builder.Services.AddScoped<MembresiaService>();
// builder.Services.AddScoped<EventoService>();
// builder.Services.AddScoped<TransBankService>();
// builder.Services.AddScoped<MantenedorService>();

builder.Services.AddSingleton<IErrorResponseBuilder, ErrorResponseBuilder>();

builder.Services.AddRateLimiter(opt =>
    opt.AddFixedWindowLimiter("login", o =>
    {
        o.PermitLimit = 5;
        o.Window = TimeSpan.FromMinutes(1);
    }));

// ── JWT ────────────────────────────────────────────────────────────────────
var jwt = builder.Configuration.GetSection("JwtSettings");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwt["Issuer"],
            ValidAudience            = jwt["Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["SecretKey"]!)),
            ClockSkew = TimeSpan.Zero
        };

        opt.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                ctx.Token = ctx.Request.Cookies["jwt"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// ── CORS ───────────────────────────────────────────────────────────────────
var origenes = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(opt =>
    opt.AddPolicy("Frontend", policy =>
        policy.WithOrigins(origenes)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()));

// ── Controllers y Swagger ──────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title   = "Club Canotaje API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name        = "Authorization",
        Type        = SecuritySchemeType.ApiKey,
        Scheme      = "Bearer",
        In          = ParameterLocation.Header,
        Description = "Bearer {tu_token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            }, []
        }
    });
});

// ── Pipeline ───────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Migraciones automáticas ────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        logger.LogInformation("Aplicando migraciones...");
        db.Database.Migrate();
        logger.LogInformation("Migraciones aplicadas correctamente.");

        logger.LogInformation("Ejecutando seed...");
        DbSeeder.Seed(db);
        logger.LogInformation("Seed completado.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al inicializar la base de datos.");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseSerilogRequestLogging();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();