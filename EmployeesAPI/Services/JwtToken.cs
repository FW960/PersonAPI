using System.IdentityModel.Tokens.Jwt;
using EmployeesAPI.Entities;
using Microsoft.IdentityModel.Tokens;

namespace EmployeesAPI.Services;

public static class JwtToken
{
    public static bool ValidateToken(string token, bool forMainToken)
    {
        var handler = new JwtSecurityTokenHandler();

        try
        {
            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidAudience = AuthOptions.AUDIENCE,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(forMainToken)
            }, out SecurityToken validated);
        }
        catch
        {
            return false;
        }
        
        return true;
    }
}