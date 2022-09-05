using AutoMapper;
using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using EmployeesAPI.Repositories.Persons;
using EmployeesAPI.Services.Persons.Interfaces;
using IdentityServer4.Extensions;
using MySqlConnector;

namespace EmployeesAPI.Services.Persons;

public class CustomersServices : ICustomerServices
{
    private readonly IMapper _ceoMapperFromDto, _ceoMapperToDto;

    private readonly CustomerRepository _repository;
    public ILogger<CustomersServices> Logger { get; set; }

    public CustomersServices(CustomerRepository repository,
        IMapper ceoMapperFromDto, IMapper ceoMapperToDto)
    {
        _repository = repository;
        _ceoMapperFromDto = ceoMapperFromDto;
        _ceoMapperToDto = ceoMapperToDto;
    }

    public bool Add(CustomerDto customerDto)
    {
        try
        {
            Logger.LogInformation("Adding a new customer to database.");

            var customer = new Customer
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Post = customerDto.Post,
                Email = customerDto.Email,
                Company = new Company
                {
                    Inn = customerDto.CompanyInn,
                }
            };

            bool result = _repository.Add(customer);

            Logger.LogInformation("New customer was added.");

            return result;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            return false;
        }
    }

    public bool Update(int id, CustomerDto customerDto)
    {
        try
        {
            Logger.LogInformation($"Updating customer {id}");

            var customer = new Customer
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Post = customerDto.Post,
                Email = customerDto.Email,
                Company = new Company
                {
                    Inn = customerDto.CompanyInn,
                }
            };

            Logger.LogInformation($"Customer {id} updated.");

            return _repository.Update(customer);
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());
            return false;
        }
    }

    public bool TryFind(int id, out CustomerDto employeeDto)
    {
        try
        {
            Logger.LogInformation($"Trying to find customer {id}");

            bool result = _repository.TryFind(id, out Customer person);

            if (result)
            {
                Logger.LogInformation($"Customer {id} found.");

                employeeDto = new CustomerDto
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Post = person.Post,
                    Email = person.Email,
                };
                try
                {
                    employeeDto.CompanyInn = person.Company.Inn;
                }
                catch
                {
                    Logger.LogInformation($"Customer {employeeDto.Email} doesn't have company");
                }

                return result;
            }

            Logger.LogInformation($"Customer {id} haven't been found.");

            employeeDto = new CustomerDto();

            return false;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            employeeDto = new CustomerDto();

            return false;
        }
    }

    public bool TryFind(string firstName, string lastName, out CustomerDto employeeDto)
    {
        try
        {
            Logger.LogInformation($"Trying to find customer {lastName}");

            bool result = _repository.TryFind(firstName, lastName, out Customer person);

            if (result)
            {
                Logger.LogInformation($"Customer {firstName} {lastName} found.");

                employeeDto = new CustomerDto
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Post = person.Post,
                    Email = person.Email,
                    CompanyInn = person.Company.Inn
                };

                return result;
            }

            Logger.LogInformation($"Customer {firstName} {lastName} haven't been found.");

            employeeDto = new CustomerDto();

            return false;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            employeeDto = new CustomerDto();

            return false;
        }
    }

    public bool Delete(int id)
    {
        try
        {
            Logger.LogInformation($"Trying to delete customer {id}.");

            bool result = _repository.Delete(id);

            if (result)
            {
                Logger.LogInformation($"Customer {id} successfully deleted");

                return result;
            }

            Logger.LogInformation($"Customer {id} haven't been deleted");

            return false;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());
            return false;
        }
    }

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<CustomerDto> employeeDtos)
    {
        try
        {
            Logger.LogInformation($"Trying to find customers starting from id {startIndex} to id {endIndex}");

            bool result = _repository.FindRange(startIndex, endIndex, out IReadOnlyCollection<Customer> persons);

            if (result)
            {
                List<CustomerDto> list = new List<CustomerDto>();

                foreach (var person in persons)
                {
                    list.Add(new CustomerDto
                    {
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        Post = person.Post,
                        Email = person.Email,
                        CompanyInn = person.Company == null ? null : person.Company.Inn
                    });
                }

                employeeDtos = list;

                return result;
            }
            else
            {
                employeeDtos = new List<CustomerDto>();

                return result;
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            employeeDtos = new List<CustomerDto>();

            return false;
        }
    }
}