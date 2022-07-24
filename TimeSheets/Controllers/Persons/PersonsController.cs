using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using TimeSheets.DTOs;
using TimeSheets.Entities;

namespace TimeSheets.Controllers.Persons;

[Route("person/manage/")]
[ApiController]
public class PersonsController : BaseController
{
    public PersonsController(MySqlConnection connection) : base(connection)
    {
    }
    
    [HttpPost("add/agent")]
    public IActionResult Add([FromBody] PersonDTO person)
    {
        return Ok();
    }

    [HttpDelete("delete/agent/{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        return Ok();
    }

    [HttpPut("update/agent/{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] PersonDTO person)
    {
        return Ok();
    }

    [HttpGet("get/by_id/agent/{id}")]
    public PersonDTO GetById([FromRoute] int id)
    {
        throw new NotImplementedException();
    }
    [HttpGet("get/by_name/agent/{name}")]
    public PersonDTO GetById([FromRoute] string name)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("get/range/agents/skip={startIndex}&take={amount}")]
    public List<PersonDTO> GetRange([FromRoute] int startIndex, [FromRoute] int amount)
    {
        throw new NotImplementedException();
    }
    
}