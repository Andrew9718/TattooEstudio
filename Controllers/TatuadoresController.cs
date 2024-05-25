using CoolTattooApi.Data;
using CoolTattooApi.Models;
using CoolTattooApi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace CoolTattooApi.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class TatuadoresController : ControllerBase
{
    private readonly ILogger<TatuadoresController> _logger;
    private readonly DataContext _context;
    private readonly WebApplicationBuilder _builder;


    public TatuadoresController(ILogger<TatuadoresController> logger, DataContext context, WebApplicationBuilder builder)
    {
        _logger = logger;
        _context = context;
        _builder = builder;

    }

    //listar todos los Tatuadores y todos sus datos.
    [HttpGet(Name = "Lista_de_Tatuadores")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Tatuador>>> GetUsuarios()
    {
        return await _context.Tatuador.ToListAsync();
    }

    // listado de Id's y nombres de los Tatuadires
    [HttpGet("NombresID", Name = "Lista_de_Nombres_Tatuadores")]
    [AllowAnonymous]

    public async Task<ActionResult<IEnumerable<object>>> GetNombresTatuadores()
    {
        var nombresTatuadores = await _context.Tatuador
                                             .Select(t => new { Id = t.Id, Nombre = t.Nombre })
                                             .ToListAsync();
        return nombresTatuadores;
    }


    //buscar un usuario  por su id en especifico.
    [HttpGet("{id}", Name = "Obtener_tatuador")]
    [TypeFilter(typeof(JwtAuthorizationRequirement))]
    public async Task<ActionResult<Tatuador>> GetUsuarioPorId(int id)
    {
        var tatuador = await _context.Tatuador.FindAsync(id);

        if (tatuador == null)
        {
            return NotFound();
        }
        return tatuador;
    }

    [HttpPost(Name = "Registrar_Tatuador")]
    [Authorize]
    [TypeFilter(typeof(JwtAuthorizationRequirement))]
    public async Task<ActionResult> RegistrarTatuador([FromForm] Tatuador tatuador, [FromForm(Name = "Foto")] IFormFile file)
    {
        try
        {
            // Verificar si se proporcionó una imagen
            if (file != null && file.Length > 0)
            {
                // Guardar los datos de la imagen en un arreglo de bytes
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    tatuador.Foto = memoryStream.ToArray();
                }
            }

            // Verificar si el correo ya está registrado
            var correoExistente = await _context.Tatuador.AnyAsync(u => u.Email == tatuador.Email);
            if (correoExistente)
            {
                return BadRequest("El correo electrónico ya está registrado");
            }

            // Verificar si el nombre de usuario ya está en uso
            var usuarioExistente = await _context.Tatuador.AnyAsync(u => u.Nombre == tatuador.Nombre);
            if (usuarioExistente)
            {
                return BadRequest("El Nombre de Usuario no está disponible");
            }

            // Encriptar la contraseña antes de almacenarla
            tatuador.Password = Encriptacion.Encriptacion.GetSHA256(tatuador.Password);

            // Guardar el nuevo tatuador en la base de datos
            _context.Tatuador.Add(tatuador);
            await _context.SaveChangesAsync();

            // Enviar correo de notificación
            var gmailSettings = _builder.Services.BuildServiceProvider().GetRequiredService<IOptions<GmailSettings>>();
            var notificacion = new Mensaje(gmailSettings);
            notificacion.EnviarCorreoRegistro(tatuador.Email, tatuador.Nombre);

            return Ok("Tatuador registrado correctamente");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // Modificar un tatuador existente de la base de datos
    [HttpPut("{id}", Name = "Modificar_Tatuador")]
    [Authorize]
    [TypeFilter(typeof(JwtAuthorizationRequirement))]
    public async Task<IActionResult> ModificarTatuador(int id, [FromForm] Tatuador tatuador, [FromForm(Name = "foto")] IFormFile? file = null)
    {
        // Verificar si el tatuador existe en la base de datos
        var tatuadorExistente = await _context.Tatuador.FindAsync(id);
        if (tatuadorExistente == null)
        {
            return BadRequest("El Tatuador no Existe.");
        }

        try
        {
            // Verificar si se proporcionó una imagen
            if (file != null && file.Length > 0)
            {
                // Guardar los datos de la imagen en un arreglo de bytes
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    tatuadorExistente.Foto = memoryStream.ToArray();
                }
            }

            // Actualizar los datos del tatuador si se proporcionaron nuevos valores
            if (!string.IsNullOrWhiteSpace(tatuador.Nombre))
            {
                tatuadorExistente.Nombre = tatuador.Nombre;
            }

            if (!string.IsNullOrWhiteSpace(tatuador.Email))
            {
                tatuadorExistente.Email = tatuador.Email;
            }

            if (!string.IsNullOrWhiteSpace(tatuador.Password))
            {
                tatuadorExistente.Password = Encriptacion.Encriptacion.GetSHA256(tatuador.Password);
            }

            if (tatuador.Telefono != 0)
            {
                tatuadorExistente.Telefono = tatuador.Telefono;
            }

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Tatuador modificado correctamente.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }



    [HttpDelete("{id}")]
    [Authorize]
    [TypeFilter(typeof(JwtAuthorizationRequirement))]
    public async Task<ActionResult> DeleteTatuador(int id)
    {
        // Incluir clientes e imágenes en la consulta
        var tatuador = await _context.Tatuador
                                     .Include(t => t.Clientes)
                                     .Include(t => t.ImagenesTattoo) // Asegúrate de que tienes una relación con las imágenes
                                     .FirstOrDefaultAsync(t => t.Id == id);

        if (tatuador == null)
        {
            return NotFound("No se encuentra este Tatuador en la Base de Datos");
        }

        // Eliminar las imágenes asociadas
        if (tatuador.ImagenesTattoo != null && tatuador.ImagenesTattoo.Any())
        {
            _context.Imagenes.RemoveRange(tatuador.ImagenesTattoo);
        }

        // Eliminar los clientes asociados
        if (tatuador.Clientes != null && tatuador.Clientes.Any())
        {
            _context.Cliente.RemoveRange(tatuador.Clientes);
        }

        // Eliminar el tatuador
        _context.Tatuador.Remove(tatuador);
        await _context.SaveChangesAsync();

        string mensaje = $"El Tatuador: {tatuador.Nombre}, sus clientes asociados y sus imágenes se eliminaron correctamente.";
        return Ok(mensaje);
    }



}



