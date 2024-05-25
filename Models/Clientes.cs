namespace CoolTattooApi.Models;

public class Clientes
{
    public int Id { get; set; }
    public string? Nombre { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public bool Publicidad { get; set; }
    public int? Telefono { get; set; }
    public int Num_Tattoos { get; set; }
    // *Propiedad de navegación
    public int TatuadorId { get; set; }
    public virtual Tatuador? Tatuador { get; init; }

}


