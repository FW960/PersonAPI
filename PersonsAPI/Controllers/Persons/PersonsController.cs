using Microsoft.AspNetCore.Mvc;
using PersonsAPI.DTOs;
using PersonsAPI.Services.Persons;

namespace PersonsAPI.Controllers.Persons;

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

    [HttpPost("add/agent")]
    public IActionResult Add([FromBody] PersonDTO person)
    {
        if (_services.Add(person))
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpDelete("delete/agent/id={id}")]
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

    [HttpPut("update/agent/id={id}")]
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

    [HttpGet("get/by_last_name/agent/last_name={name}")]
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

    [HttpGet("get/range/agents/skip_from={startIndex}&take_until={endIndex}")]
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