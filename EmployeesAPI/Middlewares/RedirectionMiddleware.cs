using EmployeesAPI.Entities;
using EmployeesAPI.Services;

namespace EmployeesAPI.Middlewares;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/admin/person/manage/authorized.html")
        {
            TokenDTO tokens = new TokenDTO
            {
                token = context.Request.Headers["Token"],
                refreshToken = context.Request.Headers["RefreshToken"]
            };

            if (!JwtToken.ValidateToken(tokens))
            {
                context.Response.StatusCode = 401;
                return;
            }
            else
            {
                context.Response.Cookies.Delete("Token");

                context.Response.Cookies.Delete("RefreshToken");

                context.Response.Cookies.Append("Token", tokens.token,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(15)
                    });

                context.Response.Cookies.Append("RefreshToken", tokens.refreshToken, new CookieOptions
                    {
                        Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(600)
                    }
                );
            }
        }

        await _next(context);
    }
}