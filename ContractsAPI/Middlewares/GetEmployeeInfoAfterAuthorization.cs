using System.Text;
using System.Text.Json;
using ContractsAPI.Services;
using MySqlConnector;

namespace ContractsAPI.Middlewares;

public class GetEmployeeInfoAfterAuthorization
{
    private RequestDelegate _next;

    public GetEmployeeInfoAfterAuthorization(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            if (context.Request.Path.Equals("/getEmployeeData"))
            {
                string email =
                    JwtToken.GetEmailFromClaims(context.Request.Headers.First(x => x.Key.Equals("Authorization"))
                        .Value.First().Split("Bearer ")[1]);

                ConfigurationManager configurationManager = new ConfigurationManager();

                configurationManager.AddJsonFile(
                    @"C:\Users\windo\RiderProjects\TimeSheets\ContractsAPI\appsettings.json");

                MySqlConnection connection = new MySqlConnection(configurationManager.GetConnectionString("default"));

                try
                {
                    connection.Open();

                    var cmd = connection.CreateCommand();

                    cmd.CommandText = "SELECT FirstName, LastName, `Group` FROM employees WHERE Email = @email";

                    cmd.Parameters.AddWithValue("@email", email);

                    var reader = cmd.ExecuteReader();

                    reader.Read();

                    dynamic employee = new
                    {
                        FirstName = reader.GetString(0),
                        LastName = reader.GetString(1),
                        Group = reader.GetInt32(2)
                    };

                    reader.Close();

                    var employeeJson = JsonSerializer.Serialize(employee);

                    byte[] bytes = Encoding.UTF8.GetBytes(employeeJson);

                    await context.Response.Body.WriteAsync(bytes);
                    
                    return;
                }
                catch
                {
                    //todo logger
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        catch
        {
            context.Response.StatusCode = 401;
        }

        await _next(context);
    }
}