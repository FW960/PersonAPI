namespace EmployeesAPI.DTOs;

public abstract class BasePersonDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}