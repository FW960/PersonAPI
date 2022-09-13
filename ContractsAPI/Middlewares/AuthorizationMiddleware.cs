using ContractsAPI.Services;

namespace ContractsAPI.Middlewares;

public class AuthorizationMiddleware
{
    private RequestDelegate _next;

    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.Equals("/authorized.html"))
        {
            try
            {
                string token = context.Request.Cookies.FirstOrDefault(x => x.Key.Equals("Token")).Value;

                string refreshToken = context.Request.Cookies.FirstOrDefault(x => x.Key.Equals("RefreshToken")).Value;

                if (!JwtToken.Validate(token, true))
                {
                    if (JwtToken.Validate(refreshToken, false))
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            HttpRequestMessage requestMessage = new HttpRequestMessage();

                            requestMessage.Method = HttpMethod.Get;
                            requestMessage.RequestUri =
                                new Uri("https://localhost:7001/authorize/employee/get-new-token");
                            requestMessage.Headers.Add("RefreshToken", refreshToken);

                            HttpResponseMessage response = await client.SendAsync(requestMessage);

                            token = response.Headers.First(x => x.Key.Equals("Token")).Value.First();
                        }

                        RewriteCookies(context, token);

                        context.Response.StatusCode = 200;
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 200;
                }
            }
            catch
            {
                context.Response.StatusCode = 401;
                return;
            }
        }

        await _next(context);
    }

    public void RewriteCookies(HttpContext context, string token)
    {
        CookieOptions options = new CookieOptions();

        context.Response.Cookies.Delete("Token");

        options.Expires = DateTime.UtcNow + TimeSpan.FromMinutes(15);

        context.Response.Cookies.Append("Token", token, options);
    }
}