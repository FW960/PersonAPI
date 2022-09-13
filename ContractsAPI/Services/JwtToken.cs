using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace ContractsAPI.Services;

public static class JwtToken
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

    public static string GetEmailFromClaims(string token)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

        string email = jwtToken.Claims.First(x => x.Type.Contains("Email")).Value;

        return email;
    }
}