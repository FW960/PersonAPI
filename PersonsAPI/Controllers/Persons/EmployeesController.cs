using System.Buffers;
using System.Text;
using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using EmployeesAPI.Services.Persons;
using EmployeesAPI.Services.Services;
using EmployeesAPI.Services.Validators.Employees;
using EmployeesAPI.Services.Validators.ErrorCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesAPI.Controllers.Persons;

[Authorize]
[Route("employees")]
[ApiController]
public class EmployeesController : BaseController
{
    private readonly EmployeeServices _services;
    public EmployeesController(EmployeeServices services, ILogger<EmployeesController> logger)
    {
        services.Logger = logger;
        _services = services;
    }

    [Authorize]
    [HttpPost("add/agent")]
    public IActionResult Add([FromBody] EmployeeDTO employee, [FromHeader] string password)
    {
        if (_services.Add(employee, password))
        {
            return Ok();
        }

        return NotFound();
    }

    [Authorize]
    [HttpDelete("delete/agent/id={id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        if (_services.Delete(id))
        {
            return Ok();
        }

        return NotFound();
    }

    [Authorize]
    [HttpPut("update/agent/id={id:int}")]
    public IActionResult Update([FromRoute] int id, [FromHeader] string password, [FromBody] EmployeeDTO employee)
    {
        if (_services.Update(id, password, employee))
        {
            return Ok();
        }

        return NotFound();
    }

    [Authorize]
    [HttpGet("get/by_id/agent/id={id}")]
    public IActionResult TryFind([FromRoute] int id)
    {
        if (_services.TryFind(id, out EmployeeDTO dto))
        {
            return Ok(dto);
        }

        return NotFound();
    }

    [Authorize]
    [HttpGet("get/by_full_name/agent/first_name={firstName:alpha}/last_name={lastName:alpha}")]
    public IActionResult TryFind([FromRoute] string firstName, [FromRoute] string lastName)
    {
        if (_services.TryFind(firstName, lastName, out EmployeeDTO dto))
        {
            return Ok(dto);
        }

        return NotFound();
    }

    [Authorize]
    [HttpGet("get/range/agents/skip_from={startIndex:int}&take_until={endIndex:int}")]
    public IActionResult GetRange([FromRoute] int startIndex, [FromRoute] int endIndex)
    {
        if (_services.FindRange(startIndex, endIndex, out IReadOnlyCollection<EmployeeDTO> dtos))
        {
            return Ok(dtos);
        }

        return NotFound();
    }
}