using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace EmployeesAPI.Entities;

public class AuthOptions
{
    public const string ISSUER = "AuthServer";
    public const string AUDIENCE = "AudienceServer";

    public const string MainTokenKEY = @"THIS IS SOME VERY SECRET
            STRING!!! Im blue da ba dee da ba di da ba dee da ba di da d ba dee da ba
        di da ba dee";

    public const string RefTokenKEY = @"THIS IS SOME VERY SECRET
            STRING!!! Im blue da ba dee da ba di da ba dee da ba di da d ba dee da ba
        di da ba dee boo";

    public static SymmetricSecurityKey GetSymmetricSecurityKey(bool forMainToken)
    {
        if (forMainToken)
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(MainTokenKEY));

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(RefTokenKEY));
    }
}