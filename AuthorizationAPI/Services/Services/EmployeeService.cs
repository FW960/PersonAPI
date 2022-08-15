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
            throw new NotImplementedException();
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