using System.IdentityModel.Tokens.Jwt;
using EmployeesAPI.Entities;
using Microsoft.IdentityModel.Tokens;

namespace EmployeesAPI.Services;

public static class JwtToken
{
    public static bool ValidateToken(TokenDTO dtos)
    {
        var handler = new JwtSecurityTokenHandler();

        try
        {
            handler.ValidateToken(dtos.token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidAudience = AuthOptions.AUDIENCE,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
            }, out SecurityToken token);
        }
        catch
        {
            return false;
        }

        try
        {
            handler.ValidateToken(dtos.refreshToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidAudience = AuthOptions.AUDIENCE,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
            }, out SecurityToken token);
        }
        catch
        {
            return false;
        }

        return true;
    }
}