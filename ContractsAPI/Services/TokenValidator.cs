using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace ContractsAPI.Services;

public static class TokenValidator
{
    public static bool Validate(string token, bool mainToken)
    {
        try
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = EmployeeAuthOptions.AUDIENCE,
                ValidateIssuer = true,
                ValidIssuer = EmployeeAuthOptions.ISSUER,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = EmployeeAuthOptions.GetSymmetricSecurityKey(mainToken)
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }
}