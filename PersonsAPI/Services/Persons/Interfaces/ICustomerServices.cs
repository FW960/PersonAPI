using EmployeesAPI.DTOs;

namespace EmployeesAPI.Services.Persons.Interfaces;

public interface ICustomerServices : IPersonsServices<CustomerDto>
{
    public bool Update(int id, CustomerDto customerDto);
    public bool Add(CustomerDto customerDto);
}