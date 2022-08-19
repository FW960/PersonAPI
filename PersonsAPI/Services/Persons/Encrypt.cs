using System.Security.Cryptography;
using System.Text;

namespace EmployeesAPI.Services.Persons;

public static class Encrypt
{
    public static string Password(string password)
    {
        SHA256 sha = SHA256.Create();

        byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

        var sb = new StringBuilder();

        for (int i = 0; i < bytes.Length; i++)
        {
            sb.Append(bytes[i].ToString("x2"));
        }

        return sb.ToString();
    }
}