using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationAPI.Services.Services;

public class EmployeeAuthOptions
{
    public const string ISSUER = "AuthServer";
    public const string AUDIENCE = "AudienceServer";

    public const string MainTokenKey = @"EMPLOYEE VERY VERY SECRET MAIN TOKEN KEY";

    public const string RefTokenKey = @"EMPLOYEE REFRESH TOKEN KEY";

    public static SymmetricSecurityKey GetSymmetricSecurityKey(bool forMainToken)
    {
        if (forMainToken)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(MainTokenKey));
        }
        else
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(RefTokenKey));
        }
    }
}