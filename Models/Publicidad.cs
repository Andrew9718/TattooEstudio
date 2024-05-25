namespace CoolTattooApi.Models
{
    public class Publicidad
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = String.Empty;
        public string Descripcion { get; set; } = String.Empty;
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public byte[]? Imagen { get; set; } = new byte[0];

    }
}