using System.ComponentModel.DataAnnotations;

namespace EmployeesAPI.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Inn { get; set; }
    public Ceo Ceo { get; set; }
}