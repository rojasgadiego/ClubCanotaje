using ClubCanotajeAPI.Context;
using ClubCanotajeAPI.Middleware;
using ClubCanotajeAPI.Repositories.CanoaRepository;
using ClubCanotajeAPI.Repositories.EventoRepository;
using ClubCanotajeAPI.Repositories.ImplementoRepository;
using ClubCanotajeAPI.Repositories.MembresiaRepository;
using ClubCanotajeAPI.Repositories.RemadorRepository;
using ClubCanotajeAPI.Repositories.SalidaRepository;
using ClubCanotajeAPI.Repositories.Usuario;
using ClubCanotajeAPI.Repositories.Verificacion;
using ClubCanotajeAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure(3)
    ));

builder.Services.AddScoped<RemadorRepository>();
builder.Services.AddScoped<CanoaRepository>();
builder.Services.AddScoped<SalidaRepository>();
builder.Services.AddScoped<ImplementoRepository>();
builder.Services.AddScoped<MembresiaRepository>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<VerificacionRepository>();
builder.Services.AddScoped<EventoRepository>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RemadorService>();
builder.Services.AddScoped<CanoaService>();
builder.Services.AddScoped<SalidaService>();
builder.Services.AddScoped<ImplementoService>();
builder.Services.AddScoped<MembresiaService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<EventoService>();



var jwt = builder.Configuration.GetSection("JwtSettings");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["SecretKey"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var origenes = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(opt =>
    opt.AddPolicy("Frontend", policy =>
        policy.WithOrigins(origenes)
              .AllowAnyHeader()
              .AllowAnyMethod()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Club Canotaje API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
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

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();