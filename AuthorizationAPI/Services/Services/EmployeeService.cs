using AuthorizationAPI.Entities;
using EmployeesAPI.Services.Persons;
using MySqlConnector;

namespace AuthorizationAPI.Services.Services;

public class EmployeeService : IService
{
    private readonly MySqlConnection _connection;

    public EmployeeService(MySqlConnection connection)
    {
        _connection = connection;
    }

    public bool Authenticate(MyAuthenticationRequest request, out TokenDTO token, HttpContext context, bool passIsEnc)
    {
        if (string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
        {
            token = new TokenDTO();

            return false;
        }

        if (!passIsEnc)
            request.Password = Encrypt.Password(request.Password);

        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "SELECT * FROM employees WHERE Email = @email AND Password = @password";

            cmd.Parameters.AddWithValue("@email", request.Login);

            cmd.Parameters.AddWithValue("@password", request.Password);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                token = new TokenDTO
                {
                    token = JwtToken.GenerateToken(request.Login, request.Password, TimeSpan.FromMinutes(15),
                        true, EmployeeAuthOptions.GetSymmetricSecurityKey, AUDIENCE: EmployeeAuthOptions.AUDIENCE,
                        ISSUER: EmployeeAuthOptions.ISSUER),
                    refreshToken = JwtToken.GenerateToken(request.Login, request.Password, TimeSpan.FromMinutes(300),
                        false, EmployeeAuthOptions.GetSymmetricSecurityKey, AUDIENCE: EmployeeAuthOptions.AUDIENCE,
                        ISSUER: EmployeeAuthOptions.ISSUER)
                };

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(15)
                };

                context.Response.Cookies.Append("MainToken", token.refreshToken, cookieOptions);

                cookieOptions.Expires = DateTimeOffset.UtcNow.AddMinutes(300);

                context.Response.Cookies.Append("RefreshToken", token.refreshToken, cookieOptions);

                return true;
            }

            token = new TokenDTO();

            return false;
        }
        catch
        {
            //todo: Logger

            token = new TokenDTO();

            return false;
        }
        finally
        {
            _connection.Close();
        }
    }
}