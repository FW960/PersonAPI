using EmployeesAPI.DTOs;

namespace EmployeesAPI.Services.Persons.Interfaces;

public interface ICustomerServices : IPersonsServices<CustomerDto>
{
    public bool Add(CustomerDto personDto);
}