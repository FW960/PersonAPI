using System.Text.Json;
using Newtonsoft.Json;

namespace EmployeesAPI.Services.Validators.ErrorCodes;

public class ErrorCodesJsonConverter : System.Text.Json.Serialization.JsonConverter<ValidationCodes>
{
    public override ValidationCodes? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ValidationCodes codes = new ValidationCodes();

        codes.Codes = new List<KeyValuePair<string, string>>();
        
        while (reader.Read())
        {
            if(reader.TokenType != JsonTokenType.PropertyName)
                continue;
            
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                reader.Read();

                if (reader.TokenType == JsonTokenType.String)
                {
                    string errorCode = reader.GetString();

                    reader.Read();

                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        reader.Read();

                        if (reader.TokenType == JsonTokenType.String)
                        {
                            string errorCodeMessage = reader.GetString();
                            
                            codes.Codes.Add(new KeyValuePair<string, string>(errorCode, errorCodeMessage));
                        }
                    }
                }
            }
        }

        return codes;
    }

    public override void Write(Utf8JsonWriter writer, ValidationCodes value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}