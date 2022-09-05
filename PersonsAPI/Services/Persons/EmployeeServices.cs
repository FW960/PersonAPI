using AutoMapper;
using EmployeesAPI.Controllers.Persons;
using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using EmployeesAPI.Repositories.Persons;
using EmployeesAPI.Repositories.Persons.Interfaces;
using EmployeesAPI.Services.Persons.Interfaces;

namespace EmployeesAPI.Services.Persons;

public class EmployeeServices : IEmployeeServices
{
    private readonly IMapper _mapperFromDto;

    private readonly IMapper _mapperToDto;

    private IPersonsRepository<Employee> _repository;
    public ILogger<EmployeesController> Logger { get; set; }

    public EmployeeServices(Mapper mapperFromDto, EmployeeRepository repository, Mapper mapperToDto)
    {
        _mapperFromDto = mapperFromDto;

        _repository = repository;

        _mapperToDto = mapperToDto;
    }

    public bool Add(EmployeeDTO employeeDto, string password)
    {
        try
        {
            Logger.LogInformation("Adding a new employee to database.");

            var person = _mapperFromDto.Map<Employee>(employeeDto);

            person.Password = Encrypt.Password(password);

            bool result = _repository.Add(person);

            Logger.LogInformation("New employee was added.");

            return result;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            return false;
        }
    }

    public bool Update(int id, string password, EmployeeDTO employeeDto)
    {
        try
        {
            Logger.LogInformation($"Updating employee {id}");

            var employee = _mapperFromDto.Map<Employee>(employeeDto);

            employee.Id = id;

            employee.Password = password;

            Logger.LogInformation($"Employee {id} updated.");

            return _repository.Update(employee);
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());
            return false;
        }
    }

    public bool TryFind(int id, out EmployeeDTO employeeDto)
    {
        try
        {
            Logger.LogInformation($"Trying to find employee {id}");

            bool result = _repository.TryFind(id, out Employee person);

            if (result)
            {
                Logger.LogInformation($"Employee {id} found.");

                employeeDto = _mapperToDto.Map<EmployeeDTO>(person);

                return result;
            }

            Logger.LogInformation($"Employee {id} haven't been found.");

            employeeDto = new EmployeeDTO();

            return false;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            employeeDto = new EmployeeDTO();

            return false;
        }
    }

    public bool TryFind(string firstName, string lastName, out EmployeeDTO employeeDto)
    {
        try
        {
            Logger.LogInformation($"Trying to find employee {lastName}");

            bool result = _repository.TryFind(firstName, lastName, out Employee person);

            if (result)
            {
                Logger.LogInformation($"Employee {lastName} found.");

                employeeDto = _mapperToDto.Map<EmployeeDTO>(person);

                return result;
            }

            Logger.LogInformation($"Employee {lastName} haven't been found.");

            employeeDto = new EmployeeDTO();

            return false;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            employeeDto = new EmployeeDTO();

            return false;
        }
    }

    public bool Delete(int id)
    {
        try
        {
            Logger.LogInformation($"Trying to delete employee {id}.");

            bool result = _repository.Delete(id);

            if (result)
            {
                Logger.LogInformation($"Employee {id} successfully deleted");

                return result;
            }

            Logger.LogInformation($"Employee {id} haven't been deleted");

            return false;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());
            return false;
        }
    }

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<EmployeeDTO> employeeDtos)
    {
        try
        {
            Logger.LogInformation($"Trying to find employees starting from id {startIndex} to id {endIndex}");

            bool result = _repository.FindRange(startIndex, endIndex, out IReadOnlyCollection<Employee> persons);

            if (result)
            {
                List<EmployeeDTO> list = new List<EmployeeDTO>();

                foreach (var person in persons)
                    list.Add(_mapperToDto.Map<EmployeeDTO>(person));

                employeeDtos = list;

                return result;
            }
            else
            {
                employeeDtos = new List<EmployeeDTO>();

                return result;
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            employeeDtos = new List<EmployeeDTO>();

            return false;
        }
    }
}