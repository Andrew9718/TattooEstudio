using CoolTattooApi.Data;
using Microsoft.EntityFrameworkCore;
using CoolTattooApi.CorseConfig;
using CoolTattooApi.Models;
using CoolTattooApi.Servicios;
using CoolTattoo_Api.TokenConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;








var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(builder);
builder.Services.AddSingleton<TokenConfig>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenConfig.secretKey))
        };
    });

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.Configure<GmailSettings>(builder.Configuration.GetSection("GmailSettings"));
builder.Services.AddTransient<IMensajes, Mensaje>();
builder.Services.AddControllers(); //registrar los controladores





// Configuración CORS
builder.Services.AddCors(options =>
{
    var corseConfig = new CorseConfig();
    var permisos = corseConfig.Permisos;

    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins(permisos)
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

//configuracion para trabajar con tokens
builder.Services.AddScoped<JwtAuthorizationRequirement>();
builder.Services.AddControllers(options =>
{
    // Agregar el filtro de acción globalmente
    options.Filters.AddService<JwtAuthorizationRequirement>();
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin"); 
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
