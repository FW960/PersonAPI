using System.Text.Json;
using EmployeesAPI.DTOs;
using EmployeesAPI.Services.Validators.ErrorCodes;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

var pers = JsonSerializer.Deserialize<EmployeeDTO>(File.ReadAllText(@"C:\Users\windo\RiderProjects\TimeSheets\Tests\NewFile1.json"));

Console.WriteLine();