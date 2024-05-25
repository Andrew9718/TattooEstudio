using CoolTattooApi.Data;
using CoolTattooApi.Models;
using CoolTattooApi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace CoolTattooApi.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class PublicidadController : ControllerBase
{
    private readonly DataContext _context;
    private readonly WebApplicationBuilder _builder;

    public PublicidadController(DataContext context, WebApplicationBuilder builder)
    {
        _context = context;
        _builder = builder;
    }

    // GET: api/ImagenesTattoo
    [HttpGet(Name = "Mostrar_Publicaciones")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Publicidad>>> GetPublicacionesTattoo()
    {
        return await _context.Publicidad.ToListAsync();
    }

    [HttpPost(Name = "Crear_Publicidad")]
    [TypeFilter(typeof(JwtAuthorizationRequirement))]
    public async Task<ActionResult> CrearPublicidad([FromForm(Name = "Imagen")] IFormFile file, [FromForm] Publicidad publicacion)
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
                    publicacion.Imagen = memoryStream.ToArray();
                }
            }

            // Verificar si la publicación ya existe
            var publicacionExistente = await _context.Publicidad.AnyAsync(u => u.Titulo == publicacion.Titulo);
            if (publicacionExistente)
            {
                return BadRequest("Esta Publicación ya existe");
            }

            // Guardar la nueva publicación en la base de datos
            _context.Publicidad.Add(publicacion);
            await _context.SaveChangesAsync();

            var gmailSettings = _builder.Services.BuildServiceProvider().GetRequiredService<IOptions<GmailSettings>>();
            var notificacion = new Mensaje(gmailSettings);

            var Clientes = await _context.Cliente.Where(c => c.Publicidad == true).ToListAsync();

            foreach (var item in Clientes)
            {
                notificacion.EnviarCorreoPublicidad(publicacion, item);
            }

            return Ok("Publicidad creada y enviada!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    //eliminar una imagen por su id
    [HttpDelete("{id}")]
    [TypeFilter(typeof(JwtAuthorizationRequirement))]
    public async Task<ActionResult<ImagenesTattoo>> DeletePublicaciones(int id)
    {
        var publicacion = await _context.Publicidad.FindAsync(id);

        if (publicacion == null)
        {
            return NotFound("ERROR! No fue posible eliminar la imagen");
        }
        string mensaje = $"La imagen con titulo: {publicacion.Titulo},del cliente se eliminó correctamente.";

        _context.Publicidad.Remove(publicacion);
        await _context.SaveChangesAsync();
        return Ok(mensaje);
    }



}


