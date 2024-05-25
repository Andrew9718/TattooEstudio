using CoolTattooApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoolTattoo_Api.TokenConfig;
using Microsoft.AspNetCore.Authorization;

namespace CoolTattooApi.Controllers;

[ApiController]
[Route("api/[controller]/")]
[AllowAnonymous]
public class CredencialesController : ControllerBase
{
    private readonly ILogger<CredencialesController> _logger;
    private readonly DataContext _context;
    private readonly WebApplicationBuilder _builder;
    private readonly TokenConfig _tokenConfig;

    public CredencialesController(ILogger<CredencialesController> logger, DataContext context, WebApplicationBuilder builder, TokenConfig tokenConfig)
    {
        _logger = logger;
        _context = context;
        _builder = builder;
        _tokenConfig = tokenConfig;
    }

    [HttpGet("VerificarCredenciales")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> VerificarCredenciales(string email, string password)
    {
        // Buscar el usuario por su correo electrónico
        var tatuador = await _context.Tatuador.FirstOrDefaultAsync(u => u.Email == email);
        // Si no se encuentra el usuario, devolver un mensaje de error
        if (tatuador == null)
        {
            return NotFound("Credenciales inválidas");
        }
        // Verificar si la contraseña coincide con la contraseña almacenada (después de aplicar el hash)
        string hashPassword = Encriptacion.Encriptacion.GetSHA256(password);

        if (tatuador.Password != hashPassword)
        {
            return BadRequest("Credenciales inválidas");
        }
        else
        {
            var token = _tokenConfig.GenerateToken(email);
            // Devolver el token como parte del cuerpo de la respuesta
            return Ok(token);
        }
    }
}