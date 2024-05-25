namespace CoolTattooApi.Servicios;

public interface IMensajes
{
    void EnviarCorreo(string destino, string asunto, string contenido);
}