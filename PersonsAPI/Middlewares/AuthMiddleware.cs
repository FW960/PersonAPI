using EmployeesAPI.DTOs;
using EmployeesAPI.Services;

namespace EmployeesAPI.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/admin/person/manage/authorized.html")
        {
            TokenDto tokens = new TokenDto
            {
                refreshToken = context.Request.Headers["RefreshToken"],
                token = context.Request.Headers["Token"]
            };

            if (string.IsNullOrEmpty(tokens.token) && string.IsNullOrEmpty(tokens.refreshToken))
                tokens = new TokenDto
                {
                    refreshToken = context.Request.Cookies["RefreshToken"],
                    token = context.Request.Cookies["Token"]
                };

            bool mainTokenValidated = JwtToken.ValidateToken(tokens.token, true);

            if (mainTokenValidated)
            {
                context.Response.Cookies.Append("Token", tokens.token,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(15),
                    });

                context.Response.Cookies.Append("RefreshToken", tokens.refreshToken, new CookieOptions
                    {
                        Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(300),
                    }
                );
            }
            else
            {
                if (JwtToken.ValidateToken(tokens.refreshToken, false))
                {
                    string token;
                    
                    try
                    {
                        HttpClient client = new HttpClient();

                        client.BaseAddress = new Uri("https://localhost:7001");

                        HttpRequestMessage message =
                            new HttpRequestMessage(HttpMethod.Get, "authorize/admin/get-new-token");

                        message.Headers.Add("RefreshToken", tokens.refreshToken);

                        var responseMessage = await client.SendAsync(message);
                        
                        token = responseMessage.Headers.FirstOrDefault(header => header.Key == "Token").Value
                            .First();
                    }
                    catch
                    {
                        //todo logger
                        context.Response.StatusCode = 401;
                        return;
                    }

                    tokens.token = token;

                    context.Response.Cookies.Append("Token", tokens.token,
                        new CookieOptions
                        {
                            Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(15),
                        });

                    context.Response.Cookies.Append("RefreshToken", tokens.refreshToken, new CookieOptions
                        {
                            Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(300),
                        }
                    );

                }
                else
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
        }

        await _next(context);
    }
}