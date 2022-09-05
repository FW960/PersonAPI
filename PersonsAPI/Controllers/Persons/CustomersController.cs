using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using EmployeesAPI.Services.Persons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesAPI.Controllers.Persons;

[Route("customers")]
public class CustomersController : BaseController
{
    private readonly CustomersServices _services;

    public CustomersController(CustomersServices services, ILogger<CustomersServices> logger)
    {
        _services = services;
        _services.Logger = logger;
    }
    
    [Authorize]
    [HttpPost("add/agent")]
    public IActionResult Add([FromBody] CustomerDto customer)
    {
        if (_services.Add(customer))
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
    public IActionResult Update([FromRoute] int id, [FromBody] CustomerDto customer)
    {
        if (_services.Update(id, customer))
        {
            return Ok();
        }

        return NotFound();
    }

    [Authorize]
    [HttpGet("get/by_id/agent/id={id}")]
    public IActionResult TryFind([FromRoute] int id)
    {
        if (_services.TryFind(id, out CustomerDto dto))
        {
            return Ok(dto);
        }

        return NotFound();
    }

    [Authorize]
    [HttpGet("get/by_full_name/agent/first_name={firstName:alpha}/last_name={lastName:alpha}")]
    public IActionResult TryFind([FromRoute] string firstName, [FromRoute] string lastName)
    {
        if (_services.TryFind(firstName, lastName, out CustomerDto dto))
        {
            return Ok(dto);
        }

        return NotFound();
    }

    [Authorize]
    [HttpGet("get/range/agents/skip_from={startIndex:int}&take_until={endIndex:int}")]
    public IActionResult GetRange([FromRoute] int startIndex, [FromRoute] int endIndex)
    {
        if (_services.FindRange(startIndex, endIndex, out IReadOnlyCollection<CustomerDto> dtos))
        {
            return Ok(dtos);
        }

        return NotFound();
    }
}