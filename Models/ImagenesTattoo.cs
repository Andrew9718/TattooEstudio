namespace CoolTattooApi.Models;

public class ImagenesTattoo
{
    public int Id { get; set; }
    public string Titulo { get; set; } = String.Empty;
    public string Descripcion { get; set; } = String.Empty;
    public byte[] Fotografia { get; set; } = new byte[0];
    public int TatuadorId { get; set; }
    public virtual Tatuador? Tatuador { get; set; } = null!;

}
