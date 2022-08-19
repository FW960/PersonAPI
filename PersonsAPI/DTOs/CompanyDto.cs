using EmployeesAPI.Entities;

namespace EmployeesAPI.DTOs;

public class CompanyDto
{
    public string Name { get; set; }
    public int Inn { get; set; }
    public CeoDto Ceo { get; set; }
}