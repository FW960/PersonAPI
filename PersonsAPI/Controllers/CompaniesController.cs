using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesAPI.Controllers;

[Route("company")]
public class CompaniesController : BaseController
{
    [HttpPost("add")]
    public IActionResult Add([FromBody] CompanyDto dto)
    {
        return Ok();
    }

    [HttpPost("update")]
    public IActionResult Update([FromBody] CompanyDto dto)
    {
        return Ok();
    }
    [HttpGet("get/by/id={id:int}")]
    public IActionResult GetById([FromRoute] int inn)
    {
        return Ok();
    }

    [HttpGet("get/by/inn={inn:int}")]
    public IActionResult GetByInn([FromRoute] int inn)
    {
        return Ok();
    }

    [HttpDelete("delete/by/id={id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        return Ok();
    }
}