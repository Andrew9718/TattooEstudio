using CoolTattooApi.Data;
using CoolTattooApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CoolTattooApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BisuteriaController : ControllerBase
    {
        private readonly ILogger<ImagenesTattooController> _logger;
        private readonly DataContext _context;

        public BisuteriaController(ILogger<ImagenesTattooController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/ImagenesTattoo
        [HttpGet(Name = "Mostrar_Bisuteria")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Bisuteria>>> GetImagenesTattoo()
        {
            return await _context.Bisuteria.ToListAsync();
        }



        //añadir una nueva imagen
        [HttpPost(Name = "Subir_Bisuteria")]
        [TypeFilter(typeof(JwtAuthorizationRequirement))]
        public async Task<ActionResult<Bisuteria>> SubirImagen([FromForm] Bisuteria Fotografia, [FromForm(Name = "Fotografia")] IFormFile file)
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
                    Fotografia.Fotografia = memoryStream.ToArray();
                }
                _context.Bisuteria.Add(Fotografia);
                await _context.SaveChangesAsync();
                return Ok("Imagen Cargada Correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        [TypeFilter(typeof(JwtAuthorizationRequirement))]
        public async Task<ActionResult<Bisuteria>> DeleteImagenesTattoo(int id)
        {
            var imagen = await _context.Bisuteria.FindAsync(id);

            if (imagen == null)
            {
                return NotFound("ERROR! No fue posible eliminar la imagen");
            }
            string mensaje = $"La imagen con titulo: {imagen.Titulo},del cliente se eliminó correctamente.";

            _context.Bisuteria.Remove(imagen);
            await _context.SaveChangesAsync();

            return Ok(mensaje);
        }
    }
}
