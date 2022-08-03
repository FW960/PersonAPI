using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EmployeesAPI.Services;

public static class JwtToken
{
    public static string Generate(int id)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        ConfigurationManager conf = new ConfigurationManager();

        conf.AddJsonFile("appsettings.json");

        var code = conf.GetSection("Secret").Value;
        
        byte[] bytes = Encoding.UTF8.GetBytes(code);

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, id.ToString())
            }),

            Expires = DateTime.UtcNow.AddMinutes(15),

            SigningCredentials = new SigningCredentials(new
                SymmetricSecurityKey(bytes), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}