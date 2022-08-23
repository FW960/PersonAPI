using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthorizationAPI.Entities;
using EmployeesAPI.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationAPI.Services.Services;

public static class JwtToken
{
    public static string GenerateToken(string login, string password, TimeSpan time, bool forMainToken, Func<bool, SymmetricSecurityKey> symmetricKey)
    {
        var claims = new List<Claim>
        {
            new Claim("Email", login),
            new Claim("Password", password)
        };
        var jwt = new JwtSecurityToken(
            issuer: AdminAuthOptions.ISSUER,
            audience: AdminAuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(time),
            signingCredentials: new SigningCredentials(symmetricKey.Invoke(forMainToken),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    
    public static bool Validate(string token, out MyAuthenticationRequest request, bool forMainToken, Func<bool, SymmetricSecurityKey> auth)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();

            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = AdminAuthOptions.AUDIENCE,
                ValidIssuer = AdminAuthOptions.ISSUER,
                IssuerSigningKey = auth.Invoke(forMainToken)
            }, out SecurityToken validatedToken);

            var claimsReader = handler.ReadJwtToken(handler.WriteToken(validatedToken));

            string email = claimsReader.Claims.First(claim => claim.Type == "Email").Value;

            string password = claimsReader.Claims.First(claim => claim.Type == "Password").Value;

            request = new MyAuthenticationRequest
            {
                Login = email,
                Password = password
            };

            return true;
        }
        catch
        {
            request = new MyAuthenticationRequest();
            return false;
        }
    }
}