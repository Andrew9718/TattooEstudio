using System.Net.Mail;
using System.Net;
using CoolTattooApi.Models;
using Microsoft.Extensions.Options;

namespace CoolTattooApi.Servicios;
public class Mensaje : IMensajes
{
    public GmailSettings _gamilSettings { get; }
    public Mensaje(IOptions<GmailSettings> gmailSettings)
    {
        _gamilSettings = gmailSettings.Value;
    }

    public void EnviarCorreoRegistro(string destino, string nombre)
    {
        var email = destino;
        var asunto = "Alta en CoolTatoo";

        var cuerpo = $@"
        <!DOCTYPE html>
        <html>
        <head>
        <title>Correo de CoolTattooo</title>
        <style>
            /* Estilos CSS para mejorar la apariencia del correo */
        body {{
        font-family: Arial, sans-serif;
        background-color: #f4f4f4;
        padding: 20px;
        }}
        .container {{
        background-color: #ffffff;
        border-radius: 10px;
        padding: 20px;
        box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
        }}
        h1 {{
        color: #333333;
        }}
        p {{
        color: #666666;
        line-height: 1.5;
        }}
         </style>
        </head>
        <body>
        <div class='container'>
            <h1>Hola, {nombre}</h1>
        <p>Te has dado de alta como Tatuador en el sitio web de <strong>CoolTattooo</strong>.</p>
        <p>Como Tatuador de Nuestro Estudio Ahora recibiras las notificacion de Solicitudes de tatuaje al Email</p>
        <p>esto incluira la informacion y contacto personal de los posibles clientes que deseen Tatuase con tigo.</p>
        </div>
        </body>
        </html>
        ";

        EnviarCorreo(email, asunto, cuerpo);

    }


    public void EnviarCorreoPublicidad(Publicidad publicidad, Clientes cliente)
    {
        var inicio = publicidad.FechaInicio?.ToString("dd/MM/yyyy") ?? "Consulta con Tu Tatuador";
        var fin = publicidad.FechaFin?.ToString("dd/MM/yyyy") ?? "Consulta con Tu Tatuador";

        var asunto = "Publicidad CoolTattoo";
        var correo = cliente.Email;
        var nombre = cliente.Nombre;
        var imagenBase64 = Convert.ToBase64String(publicidad.Imagen);

        var cuerpo = $@"
        <!DOCTYPE html>
        <html>
        <head>
        <title>Correo de CoolTattoo</title>
        <style>
            /* Estilos CSS para mejorar la apariencia del correo */
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                padding: 20px;
            }}
            .container {{
                background-color: #ffffff;
                border-radius: 10px;
                padding: 20px;
                box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
            }}
            h1 {{
                color: #333333;
            }}
            p {{
                color: #666666;
                line-height: 1.5;
            }}
            img {{
                width: 50%;
                height: auto;
            }}
        </style>
        </head>
        <body>
        <div class='container'>
            <h1>Hola, {nombre}</h1>
            <p>Te escribimos para contarte las nuevas noticias de <strong>CoolTattoo</strong>.</p>
            <p>¡Esperamos que disfrutes de nuestras novedades!</p><br/>
            <p>{publicidad.Titulo}</p>
            <img src='data:image/jpeg;base64,{imagenBase64}' alt=''>
            <p>{publicidad.Descripcion}</p>
            <p>Desde: {inicio}</p>
            <p>Hasta: {fin}</p>
            <p>¡Esperamos verte pronto!</p><br/>
        </div>
        </body>
        </html>
    ";

        EnviarCorreo(correo, asunto, cuerpo);
    }


    public void EnviarCorreoClienteFrecuente(string destino, string nombre)
    {
        var email = destino;
        var asunto = "¡Hola de nuevo desde CoolTatoo!";

        var cuerpo = $@"
    <!DOCTYPE html>
    <html>
    <head>
    <title>Correo de CoolTattooo</title>
    <style>
        /* Estilos CSS para mejorar la apariencia del correo */
    body {{
    font-family: Arial, sans-serif;
    background-color: #f4f4f4;
    padding: 20px;
    }}
    .container {{
    background-color: #ffffff;
    border-radius: 10px;
    padding: 20px;
    box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
    }}
    h1 {{
    color: #333333;
    }}
    p {{
    color: #666666;
    line-height: 1.5;
    }}
     </style>
    </head>
    <body>
    <div class='container'>
        <h1>Hola, {nombre}</h1>
    <p>¡Te damos la bienvenida de nuevo a CoolTattoo!</p>
    <p>Gracias por confiar en nosotros como tu estudio de tatuajes preferido.</p>
    <p>Siempre estamos aquí para ayudarte con tus nuevas ideas de tatuajes.</p>
    <p>En breve tu Tatuador se pondra en contacto con tigo para afinar los detalles de tu nuevo Tatoo</p>
    </div>
    </body>
    </html>
    ";

        EnviarCorreo(email, asunto, cuerpo);
    }

    public void EnviarCorreoClienteNuevo(string destino, string nombre)
    {
        var email = destino;
        var asunto = "¡Bienvenido a CoolTatoo!";

        var cuerpo = $@"
    <!DOCTYPE html>
    <html>
    <head>
    <title>Correo de CoolTattoo</title>
    <style>
        /* Estilos CSS para mejorar la apariencia del correo */
    body {{
    font-family: Arial, sans-serif;
    background-color: #f4f4f4;
    padding: 20px;
    }}
    .container {{
    background-color: #ffffff;
    border-radius: 10px;
    padding: 20px;
    box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
    }}
    h1 {{
    color: #333333;
    }}
    p {{
    color: #666666;
    line-height: 1.5;
    }}
     </style>
    </head>
    <body>
    <div class='container'>
        <h1>Hola, {nombre}</h1>
    <p>¡Bienvenido a CoolTattoo!</p>
    <p>Gracias por elegirnos como tu estudio de tatuajes.</p>
    <p>Estamos emocionados de ayudarte a expresar tu estilo único a través de nuestros tatuajes.</p>
    <p>En Breve el Tu Tatuador se pondra encontacto con tigo.</p>
    </div>
    </body>
    </html>
    ";
        EnviarCorreo(email, asunto, cuerpo);
    }

    public void EnviarCorreoTatuador(string destino, string clienteNombre, string clienteEmail, int? clienteTelefono, bool clienteNuevo)
    {
        var email = destino;
        var asunto = "Solicitud de Tatuaje";

        var cuerpo = $@"
    <!DOCTYPE html>
    <html>
    <head>
    <title>Correo de CoolTattoo</title>
    <style>
        /* Estilos CSS para mejorar la apariencia del correo */
    body {{
    font-family: Arial, sans-serif;
    background-color: #f4f4f4;
    padding: 20px;
    }}
    .container {{
    background-color: #ffffff;
    border-radius: 10px;
    padding: 20px;
    box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
    }}
    h1 {{
    color: #333333;
    }}
    p {{
    color: #666666;
    line-height: 1.5;
    }}
     </style>
    </head>
    <body>
    <div class='container'>
        <h1>Solicitud de Tatuaje</h1>
        <p>Estimado Tatuador,</p>
        <p>El cliente <strong>{clienteNombre}</strong> ha solicitado un tatuaje.</p>
        <p>Datos del cliente:</p>
        <ul>
            <li>Nombre: {clienteNombre}</li>
            <li>Email: {clienteEmail}</li>
            <li>Movil: {clienteTelefono}</li>
            <li>Estado: {(clienteNuevo ? "Nuevo cliente" : "Cliente frecuente")}</li>
        </ul>
        <p>Por favor, ponte en contacto con el cliente para discutir los detalles del tatuaje.</p>
    </div>
    </body>
    </html>
    ";
        EnviarCorreo(email, asunto, cuerpo);
    }




    public void EnviarCorreo(string destino, string asunto, string contenido)
    {
        try
        {
            var email = _gamilSettings.Correo;
            var pass = _gamilSettings.Password;
            var mensaje = new MailMessage();
            mensaje.From = new MailAddress(email!);
            mensaje.Subject = asunto;
            mensaje.To.Add(new MailAddress(destino));
            mensaje.Body = contenido;
            mensaje.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = _gamilSettings.Port,
                Credentials = new NetworkCredential(email, pass),
                EnableSsl = true


            };
            smtpClient.Send(mensaje);

        }
        catch (System.Exception)
        {
            throw new Exception("No se ha enviado el Email");
        }
    }
}
