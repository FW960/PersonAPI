using System.Text.Json;

namespace EmployeesAPI.Services.Validators.ErrorCodes;

public class ValidationCodes
{
    public List<KeyValuePair<string, string>> Codes { get; set; }

    public string GetCodeMessage(string code)
    {
        string errorMessage = Codes.FirstOrDefault(x => x.Key.Equals(code)).Value;

        return errorMessage;
    }

    public static ValidationCodes  Create()
    {
        string json =
            File.ReadAllText(
                @"C:\Users\windo\RiderProjects\TimeSheets\PersonsAPI\Services\Validators\ErrorCodes\ValidationErrorCodes.Json");

        JsonSerializerOptions options = new JsonSerializerOptions();
        
        options.Converters.Add(new ErrorCodesJsonConverter());

        var obj = JsonSerializer.Deserialize<ValidationCodes>(json, options);

        return obj;
    }
}