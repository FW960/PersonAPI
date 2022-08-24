using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContractsAPI.Controllers;

[Route("contracts")]
public class ContractsController : Controller
{
    [Authorize]
    [HttpGet("get/by-name={name:alpha}")]
    public IActionResult Get([FromRoute] string name)
    {
        return Ok();
    }

    [Authorize]
    [HttpDelete("delete/by-name={name:alpha}")]
    public IActionResult Delete([FromRoute] string name)
    {
        return Ok();
    }

    [Authorize]
    [HttpPost("add")]
    public IActionResult Add()
    {
        //todo Server must download file from http context and upload to directory
        return Ok();
    }

    [Authorize]
    [HttpPut("update/by-name={name:alpha}")]
    public IActionResult Update([FromRoute] string name)
    {
        //todo Server must download file from http context and upload to directory
        return Ok();
    }

}