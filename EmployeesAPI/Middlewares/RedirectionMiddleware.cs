using System.Text.Json;
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
                refreshToken = context.Request.Headers["RefreshToken"],
                token = context.Request.Headers["Token"]
            };

            if (string.IsNullOrEmpty(tokens.token) && string.IsNullOrEmpty(tokens.refreshToken))
                tokens = new TokenDTO
                {
                    refreshToken = context.Request.Cookies["RefreshToken"],
                    token = context.Request.Cookies["Token"]
                };

            bool validateToken = JwtToken.ValidateToken(tokens.token);

            if (validateToken)
            {
                context.Response.Cookies.Append("Token", tokens.token,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(15),
                    });

                context.Response.Cookies.Append("RefreshToken", tokens.refreshToken, new CookieOptions
                    {
                        Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(600),
                    }
                );
            }
            else
            {
                if (JwtToken.ValidateToken(tokens.refreshToken))
                {
                    HttpClient client = new HttpClient();

                    HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get,
                        "https://localho.st:7001/authorize/admin/get-new-token");

                    message.Headers.Add("RefreshToken", tokens.refreshToken);

                    var headers = client.Send(message).Headers;

                    tokens.token = headers.First(header => header.Key == "Token").Value.ToString();

                    context.Response.Cookies.Append("Token", tokens.token,
                        new CookieOptions
                        {
                            Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(15),
                        });

                    context.Response.Cookies.Append("RefreshToken", tokens.refreshToken, new CookieOptions
                        {
                            Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(600),
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