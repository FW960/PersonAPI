using AuthorizationAPI.Entities;
using AuthorizationAPI.Services.Services;
using EmployeesAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Controllers;

[Route("authorize")]
[ApiController]
public class AuthorizationController : Controller
{
    private readonly IService _employeeService;

    private readonly IService _adminService;

    public AuthorizationController(EmployeeService employeeService, AdminService adminService)
    {
        _employeeService = employeeService;
        _adminService = adminService;
    }

    [AllowAnonymous]
    [HttpPost("employee")]
    public IActionResult AuthorizeEmployee([FromBody] MyAuthenticationRequest request)
    {
        if (_employeeService.Authenticate(request, out TokenDTO token, HttpContext, false))
        {
            return Ok(token);
        }

        return NotFound();
    }

    [AllowAnonymous]
    [HttpGet("employee/get-new-token")]
    public IActionResult GetNewTokenEmployee([FromHeader(Name = "RefreshToken")] string token)
    {
        if (JwtToken.Validate(token, out MyAuthenticationRequest request, false,
                EmployeeAuthOptions.GetSymmetricSecurityKey, AUDIENCE: EmployeeAuthOptions.AUDIENCE,
                ISSUER: EmployeeAuthOptions.ISSUER))
        {
            if (_employeeService.Authenticate(request, out TokenDTO dto, HttpContext, true))
            {
                Response.Headers.Append("Token", dto.token);
                return Ok(dto);
            }
        }

        return NotFound();
    }

    [AllowAnonymous]
    [HttpPost("admin")]
    public IActionResult AuthorizeAdmin([FromBody] MyAuthenticationRequest request)
    {
        if (_adminService.Authenticate(request, out TokenDTO token, HttpContext, false))
        {
            return Ok(token);
        }

        return NotFound();
    }

    [AllowAnonymous]
    [HttpGet("admin/get-new-token")]
    public IActionResult GetNewTokenAdmin([FromHeader(Name = "RefreshToken")] string token)
    {
        if (JwtToken.Validate(token, out MyAuthenticationRequest request, false,
                AdminAuthOptions.GetSymmetricSecurityKey, AUDIENCE: AdminAuthOptions.AUDIENCE,
                ISSUER: AdminAuthOptions.ISSUER))
        {
            if (_adminService.Authenticate(request, out TokenDTO dto, HttpContext, true))
            {
                Response.Headers.Append("Token", dto.token);
                return Ok();
            }
        }

        return BadRequest();
    }
}