using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EmployeesAPI.Entities;

public class AuthOptions
{
    public const string ISSUER = "AuthServer";
    public const string AUDIENCE = "AudienceServer";

    public const string KEY = @"THIS IS SOME VERY SECRET
            STRING!!! Im blue da ba dee da ba di da ba dee da ba di da d ba dee da ba
        di da ba dee";

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}