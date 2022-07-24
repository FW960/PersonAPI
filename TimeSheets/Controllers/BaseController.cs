using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace TimeSheets.Controllers;

public class BaseController : Controller
{
    protected readonly MySqlConnection _connection;
    public BaseController(MySqlConnection connection)
    {
        _connection = connection;
    }
}