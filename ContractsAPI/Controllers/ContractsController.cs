using ContractsAPI.Dto;
using ContractsAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContractsAPI.Controllers;

[Route("contracts")]
public class ContractsController : Controller
{
    private readonly ContractServices _services;
    public ContractsController(ContractServices services)
    {
        _services = services;
    } 
    
    [Authorize]
    [HttpGet("get/by-id={id:int}_and_inn={inn:int}")]
    public IActionResult Get([FromRoute] int id, [FromRoute] int inn)
    {
        if (_services.Get(HttpContext, inn, id))
        {
            return StatusCode(200);
        }

        return StatusCode(400);
    }

    [Authorize]
    [HttpDelete("delete")]
    public IActionResult Delete([FromBody] ContractDto dto)
    {
        if (_services.Delete(HttpContext, dto))
        {
            return StatusCode(200);
        }

        return StatusCode(400);
    }

    [Authorize]
    [HttpPost("add")]
    public IActionResult Add([FromBody] ContractDto dto)
    {
        if (_services.Add(HttpContext, dto, out int id))
        {
            return Ok(id);
        }
        
        return StatusCode(400);
    }

    [Authorize]
    [HttpPut("update")]
    public IActionResult Update([FromBody] ContractDto dto)
    {
        if (_services.Update(HttpContext, dto))
        {
            return StatusCode(200);
        }

        return StatusCode(400);
    }

    [Authorize]
    [HttpGet("get/for-employee")]
    public IActionResult Get()
    {
        return Ok();
    }

}