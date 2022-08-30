using EmployeesAPI.Entities;

namespace EmployeesAPI.DTOs;

public class CompanyDto
{
    public string Name { get; set; }
    public string Inn { get; set; }
    public CeoDto Ceo { get; set; }
}