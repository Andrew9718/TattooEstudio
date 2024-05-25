using System.ComponentModel.DataAnnotations.Schema;

namespace CoolTattooApi.Models
{
    public class Tatuador
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string? Password { get; set; }
        public int Telefono { get; set; }
        public byte[]? Foto { get; set; } = new byte[0];
        public virtual ICollection<Clientes>? Clientes { get; init; }
        public virtual ICollection<ImagenesTattoo>? ImagenesTattoo { get; init; }
    }
}