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
public class ClientesController : ControllerBase
{
    private readonly ILogger<ClientesController> _logger;
    private readonly DataContext _context;
    private readonly WebApplicationBuilder _builder;

    public ClientesController(ILogger<ClientesController> logger, DataContext context, WebApplicationBuilder builder)
    {
        _logger = logger;
        _context = context;
        _builder = builder;
    }

    //configuracion de la ruta para listar todos los clientes de la app.

    [HttpGet(Name = "Lista_de_Clientes")]
    [TypeFilter(typeof(JwtAuthorizationRequirement))]
    public async Task<ActionResult<IEnumerable<Clientes>>> GetUsuarios()
    {
        return await _context.Cliente.ToListAsync();
    }

    //buscar un usuario  por su id en especifico.

    [HttpGet("{id}", Name = "Obtener_Cliente")]
    [TypeFilter(typeof(JwtAuthorizationRequirement))]
    public async Task<ActionResult<Clientes>> GetUsuarioPorId(int id)
    {
        var cliente = await _context.Cliente.FindAsync(id);

        if (cliente == null)
        {
            return NotFound();
        }
        return cliente;
    }

    // Crear un nuevo cliente en la base de datos o actualizar el número de tatuajes si el cliente ya existe
    [HttpPost(Name = "Registrar_Cliente")]
    [AllowAnonymous]
    public async Task<ActionResult<Clientes>> RegistrarCliente([FromBody] Clientes cliente)
    {
        try
        {
            // Verificar si el cliente ya existe en la base de datos por su correo electrónico
            var clienteExistente = await _context.Cliente.Include(c => c.Tatuador).FirstOrDefaultAsync(c => c.Email == cliente.Email);
            if (clienteExistente != null)
            {
                // Actualizar el número de tatuajes del cliente existente
                clienteExistente.Num_Tattoos = clienteExistente.Num_Tattoos + 1;
                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
                // Enviar correo de cliente frecuente
                var gmailSettings = _builder.Services.BuildServiceProvider().GetRequiredService<IOptions<GmailSettings>>();
                var notificacion = new Mensaje(gmailSettings);
                notificacion.EnviarCorreoClienteFrecuente(cliente.Email, cliente.Nombre ?? "Estimado Cliente");
                if (clienteExistente.Tatuador != null)
                {
                    notificacion.EnviarCorreoTatuador(clienteExistente.Tatuador.Email, clienteExistente.Nombre ?? "", clienteExistente.Email, clienteExistente.Telefono, false);
                }
                return Ok($"El número de tatuajes del cliente {cliente.Nombre} ha sido actualizado correctamente.");
            }
            else
            {
                // Cliente no encontrado, agregar como nuevo cliente
                var tatuador = await _context.Tatuador.FirstOrDefaultAsync(t => t.Id == cliente.TatuadorId);//buscamos al tatuador seleccionado
                cliente.Num_Tattoos = 1;
                _context.Cliente.Add(cliente);
                await _context.SaveChangesAsync();
                // Enviar correo de cliente nuevo
                var gmailSettings = _builder.Services.BuildServiceProvider().GetRequiredService<IOptions<GmailSettings>>();
                var notificacion = new Mensaje(gmailSettings);
                notificacion.EnviarCorreoClienteNuevo(cliente.Email, cliente.Nombre ?? "Estimado Cliente");
                if (tatuador != null)
                {
                    notificacion.EnviarCorreoTatuador(tatuador.Email, cliente.Nombre ?? "", cliente.Email, cliente.Telefono, true);
                }
                return Ok($"Cliente {cliente.Nombre} registrado correctamente.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // eliminar un Cliente por su id
    [HttpDelete("{id}")]
    [TypeFilter(typeof(JwtAuthorizationRequirement))]
    public async Task<ActionResult<Clientes>> DeleteCliente(int id)
    {
        var cliente = await _context.Cliente.FindAsync(id);
        if (cliente == null)
        {
            return NotFound("No se encuentra este Cliente");
        }
        string mensaje = $"El Clinte: {cliente.Nombre} se eliminó correctamente.";
        _context.Cliente.Remove(cliente);
        await _context.SaveChangesAsync();
        return Ok(mensaje);
    }
}



