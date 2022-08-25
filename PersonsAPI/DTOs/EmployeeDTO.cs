namespace EmployeesAPI.DTOs;

public class EmployeeDTO : BasePersonDTO
{
    public int Age { get; set; }
    
    public int Group { get; set; }
}