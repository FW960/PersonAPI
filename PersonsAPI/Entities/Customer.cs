namespace EmployeesAPI.Entities;

public class Customer : BasePersonEntity
{
    public string Post { get; set; }
    public Company? Company { get; set; }
}