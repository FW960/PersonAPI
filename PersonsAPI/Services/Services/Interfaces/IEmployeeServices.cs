using EmployeesAPI.DTOs;

namespace EmployeesAPI.Services.Services.Interfaces;

public interface IEmployeeServices : IPersonsServices<EmployeeDTO>
{
    public bool Update(int id, string password, EmployeeDTO employeeDto);
    public bool Add(EmployeeDTO employeeDto, string password);
}