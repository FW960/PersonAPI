using EmployeesAPI.DTOs;

namespace EmployeesAPI.Services.Persons.Interfaces;

public interface IEmployeeServices : IPersonsServices<EmployeeDTO>
{
    public bool Add(EmployeeDTO personDto, string password);
}