namespace CoolTattooApi.Models;

public class Bisuteria
{
    public int Id { get; set; }
    public string Titulo { get; set; } = String.Empty;
    public string Descripcion { get; set; } = String.Empty;
    public byte[] Fotografia { get; set; } = new byte[0];
}
