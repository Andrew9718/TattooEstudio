using Microsoft.AspNetCore.Mvc.Filters;
using CoolTattoo_Api.TokenConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
// Clase de filtro de autorización personalizado
public class JwtAuthorizationRequirement : AuthorizeFilter
{
    private readonly TokenConfig _tokenConfig;

    public JwtAuthorizationRequirement(TokenConfig tokenConfig)
    {
        _tokenConfig = tokenConfig;
    }

#pragma warning disable CS0114 // El miembro oculta el miembro heredado. Falta una contraseña de invalidación
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
#pragma warning restore CS0114 // El miembro oculta el miembro heredado. Falta una contraseña de invalidación
    {
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token) || !_tokenConfig.ValidateToken(token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // El token es válido, continuar con la ejecución de la acción
    }
}

