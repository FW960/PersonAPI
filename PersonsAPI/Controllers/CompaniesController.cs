﻿using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using EmployeesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesAPI.Controllers;

[Route("company")]
public class CompaniesController : BaseController
{
    private readonly CompanyServices _services;

    public CompaniesController(CompanyServices services, ILogger<CompaniesController> logger)
    {
        _services = services;
        _services.Logger = logger;
    }
    [Authorize]
    [HttpPost("add")]
    public IActionResult Add([FromBody] CompanyDto companyDto)
    {
        if (_services.Add(companyDto))
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }
    [Authorize]
    [HttpPut("update/new-ceo/id={id:int}")]
    public IActionResult Update([FromBody] CompanyDto companyDto, [FromRoute] int id)
    {
        if (_services.Update(companyDto, id))
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }
    [Authorize]
    [HttpGet("get/by/inn={inn:alpha}")]
    public IActionResult GetByInn([FromRoute] string inn)
    {
        if (_services.TryFind(inn, out CompanyDto companyDto))
        {
            return Ok(companyDto);
        }
        else
        {
            return BadRequest();
        }
    }

    [Authorize]
    [HttpDelete("delete/by/inn={inn:int}")]
    public IActionResult Delete([FromRoute] string inn)
    {
        if (_services.Delete(inn))
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }
}