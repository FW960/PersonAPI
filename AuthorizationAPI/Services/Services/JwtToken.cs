using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeesAPI.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationAPI.Services.Services;

public static class JwtToken
{
    public static string GenerateToken(string login, string password, TimeSpan time, bool forMainToken)
    {
        var claims = new List<Claim>
        {
            new Claim("Email", login),
            new Claim("Password", password)
        };
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(time),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(forMainToken),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}