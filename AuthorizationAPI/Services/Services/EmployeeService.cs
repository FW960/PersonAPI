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
    public bool Authenticate(MyAuthenticationRequest request, out TokenDTO token, HttpContext context)
    {
        if (string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
        {
            token = new TokenDTO();
            
            return false;
        }

        request.Password = Encrypt.Password(request.Password);

        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText = "SELECT Id FROM employees WHERE Email = @email AND Password = @pass";

            cmd.Parameters.AddWithValue("@email", request.Login);

            cmd.Parameters.AddWithValue("@pass", request.Password);
            
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                token = new TokenDTO
                {
                    token = JwtToken.GenerateBaseToken(request.Login, reader.GetInt32(0)),
                    refreshToken = JwtToken.GenerateRefreshToken(request.Login, reader.GetInt32(0))
                };
                context.Response.Cookies.Append("MainToken", token.token);
                
                context.Response.Cookies.Append("RefreshToken", token.refreshToken);
                
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