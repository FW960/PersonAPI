namespace EmployeesAPI.Entities;

public class Employee : BasePersonEntity
{
    public int Age { get; set; }
    public string Password { get; set; }
    public int Group { get; set; }
}
