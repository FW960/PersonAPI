using EmployeesAPI.DTOs;
using EmployeesAPI.Services.Persons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesAPI.Controllers.Persons;

[Authorize]
[Route("person/manage/")]
[ApiController]
public class PersonsController : BaseController
{
    private readonly IPersonsServices _services;

    public PersonsController(PersonsServices services, ILogger<PersonsController> logger)
    {
        services.Logger = logger;
        _services = services;
    }
    [Authorize]
    [HttpPost("add/agent")]
    public IActionResult Add([FromBody] PersonDTO person, [FromQuery] string password)
    {
        if (_services.Add(person, password))
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }
    [Authorize]
    [HttpDelete("delete/agent/id={id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        if (_services.Delete(id))
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }
    [Authorize]
    [HttpPut("update/agent/id={id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] PersonDTO person)
    {
        if (_services.Update(id, person))
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }
    [Authorize]
    [HttpGet("get/by_id/agent/id={id}")]
    public IActionResult TryFind([FromRoute] int id)
    {
        if (_services.TryFind(id, out PersonDTO dto))
        {
            return Ok(dto);
        }
        else
        {
            return NotFound();
        }
    }
    [Authorize]
    [HttpGet("get/by_last_name/agent/last_name={name:alpha}")]
    public IActionResult TryFind([FromRoute] string name)
    {
        if (_services.TryFind(name, out PersonDTO dto))
        {
            return Ok(dto);
        }
        else
        {
            return NotFound();
        }
    }

    [Authorize]
    [HttpGet("get/range/agents/skip_from={startIndex:int}&take_until={endIndex:int}")]
    public IActionResult GetRange([FromRoute] int startIndex, [FromRoute] int endIndex)
    {
        if (_services.FindRange(startIndex, endIndex, out IReadOnlyCollection<PersonDTO> dtos))
        {
            return Ok(dtos);
        }
        else
        {
            return NotFound();
        }
    }
}