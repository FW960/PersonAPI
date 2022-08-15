using AuthorizationAPI.Entities;
using AuthorizationAPI.Services.Services;
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
        if (_employeeService.Authenticate(request, out TokenDTO token, HttpContext))
        {
            return Ok(token);
        }

        return NotFound();
    }

    [AllowAnonymous]
    [HttpPost("admin")]
    public IActionResult AuthorizeAdmin([FromBody] MyAuthenticationRequest request)
    {
        if (_adminService.Authenticate(request, out TokenDTO token, HttpContext))
        {
            HttpContext.Response.Cookies.Append("MainTokens", token.token);

            HttpContext.Response.Cookies.Append("RefreshTokens", token.refreshToken);

            return Ok(token);
        }

        return NotFound();
    }

    [AllowAnonymous]
    [HttpGet("/admin/get-new-token")]
    public IActionResult GetNewTokenAdmin()
    {
        var refToken = HttpContext.Request.Headers["RefreshToken"];
        
        if (ValidateToken.Admin(refToken, out MyAuthenticationRequest request))
        {
            if (_adminService.Authenticate(request, out TokenDTO dto, HttpContext))
            {
                HttpContext.Response.Headers.Add("Token", dto.token);
                return Ok();
            }    
        }
        
        return BadRequest();
    }
}