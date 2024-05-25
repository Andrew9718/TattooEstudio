using CoolTattooApi.Data;
using CoolTattooApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CoolTattooApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagenesTattooController : ControllerBase
    {
        private readonly ILogger<ImagenesTattooController> _logger;
        private readonly DataContext _context;

        public ImagenesTattooController(ILogger<ImagenesTattooController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/ImagenesTattoo
        [HttpGet(Name = "Mostrar_Imagenes")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ImagenesTattoo>>> GetImagenesTattoo()
        {
            return await _context.Imagenes.ToListAsync();
        }


        //Get Con filtro GET: api/ImagenesTattoo

        [HttpGet("Imagenes/{nombre}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ImagenesTattoo>>> GetImagenesTattooPorTatuador(string nombre)
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            var imagenes = await _context.Imagenes
                .Where(i => i.Tatuador.Nombre == nombre)
                .ToListAsync();
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

            if (imagenes == null || imagenes.Count == 0)
            {
                return NotFound();
            }
            return imagenes;
        }


        //añadir una nueva imagen
        [HttpPost(Name = "Subir_Imagen")]
        [TypeFilter(typeof(JwtAuthorizationRequirement))]
        public async Task<ActionResult<ImagenesTattoo>> SubirImagen([FromForm] ImagenesTattoo imagen, [FromForm(Name = "Fotografia")] IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("No se ha proporcionado ninguna imagen.");
            }
            try
            {
                // Guardar los datos de la imagen en un arreglo de bytes
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    imagen.Fotografia = memoryStream.ToArray();
                }
                _context.Imagenes.Add(imagen);
                await _context.SaveChangesAsync();
                return Ok("Imagen Cargada Correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        //eliminar una imagen por su id
        [HttpDelete("{id}")]
        [TypeFilter(typeof(JwtAuthorizationRequirement))]
        public async Task<ActionResult<ImagenesTattoo>> DeleteImagenesTattoo(int id)
        {
            var imagen = await _context.Imagenes.FindAsync(id);

            if (imagen == null)
            {
                return NotFound("ERROR! No fue posible eliminar la imagen");
            }
            string mensaje = $"La imagen con titulo: {imagen.Titulo},del cliente se eliminó correctamente.";

            _context.Imagenes.Remove(imagen);
            await _context.SaveChangesAsync();

            return Ok(mensaje);
        }
    }
}
