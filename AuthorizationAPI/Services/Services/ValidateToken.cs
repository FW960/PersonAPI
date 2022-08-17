using System.IdentityModel.Tokens.Jwt;
using AuthorizationAPI.Entities;
using EmployeesAPI.Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationAPI.Services.Services;

public class ValidateToken
{
    public static bool Admin(string token, out MyAuthenticationRequest request, bool forMainToken)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();

            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = AuthOptions.AUDIENCE,
                ValidIssuer = AuthOptions.ISSUER,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(forMainToken)
            }, out SecurityToken validatedToken);

            var claimsReader = handler.ReadJwtToken(token);

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