using AutoMapper;
using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using EmployeesAPI.Repositories.Persons;
using EmployeesAPI.Services.Persons.Interfaces;
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

    public bool Add(CustomerDto personDto)
    {
        try
        {
            Logger.LogInformation("Adding a new customer to database.");

            var person = new Customer
            {
                FirstName = personDto.FirstName,
                LastName = personDto.LastName,
                Post = personDto.Post,
                Email = personDto.Email,
                Company = new Company
                {
                    Inn = personDto.Company.Inn,
                    Ceo = _ceoMapperFromDto.Map<Ceo>(personDto.Company.Ceo),
                    Name = personDto.Company.Name
                }
            };

            bool result = _repository.Add(person);

            Logger.LogInformation("New customer was added.");

            return result;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            return false;
        }
    }

    public bool Update(int id, CustomerDto personDto)
    {
        try
        {
            Logger.LogInformation($"Updating customer {id}");

            var person = new Customer
            {
                FirstName = personDto.FirstName,
                LastName = personDto.LastName,
                Post = personDto.Post,
                Email = personDto.Email,
                Company = new Company
                {
                    Inn = personDto.Company.Inn,
                    Ceo = _ceoMapperFromDto.Map<Ceo>(personDto.Company.Ceo),
                    Name = personDto.Company.Name
                }
            };

            Logger.LogInformation($"Customer {id} updated.");

            return _repository.Update(id, person);
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());
            return false;
        }
    }

    public bool TryFind(int id, out CustomerDto personDto)
    {
        try
        {
            Logger.LogInformation($"Trying to find customer {id}");

            bool result = _repository.TryFind(id, out Customer person);

            if (result)
            {
                Logger.LogInformation($"Customer {id} found.");

                personDto = new CustomerDto
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Post = person.Post,
                    Email = person.Email,
                    Company = new CompanyDto
                    {
                        Inn = person.Company.Inn,
                        Ceo = _ceoMapperToDto.Map<CeoDto>(person.Company.Ceo),
                        Name = person.Company.Name
                    }
                };

                return result;
            }

            Logger.LogInformation($"Customer {id} haven't been found.");

            personDto = new CustomerDto();

            return false;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            personDto = new CustomerDto();

            return false;
        }
    }

    public bool TryFind(string firstName, string lastName, out CustomerDto personDto)
    {
        try
        {
            Logger.LogInformation($"Trying to find customer {lastName}");

            bool result = _repository.TryFind(firstName, lastName, out Customer person);

            if (result)
            {
                Logger.LogInformation($"Customer {firstName} {lastName} found.");

                personDto = new CustomerDto
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Post = person.Post,
                    Email = person.Email,
                    Company = new CompanyDto
                    {
                        Inn = person.Company.Inn,
                        Ceo = _ceoMapperToDto.Map<CeoDto>(person.Company.Ceo),
                        Name = person.Company.Name
                    }
                };

                return result;
            }

            Logger.LogInformation($"Customer {firstName} {lastName} haven't been found.");

            personDto = new CustomerDto();

            return false;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            personDto = new CustomerDto();

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

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<CustomerDto> dtos)
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
                        Company = new CompanyDto
                        {
                            Inn = person.Company.Inn,
                            Ceo = _ceoMapperToDto.Map<CeoDto>(person.Company.Ceo),
                            Name = person.Company.Name
                        }
                    });
                }

                dtos = list;

                return result;
            }
            else
            {
                dtos = new List<CustomerDto>();

                return result;
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            dtos = new List<CustomerDto>();

            return false;
        }
    }
}