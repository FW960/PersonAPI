using AutoMapper;
using EmployeesAPI.Controllers.Persons;
using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using EmployeesAPI.Repositories.Persons;

namespace EmployeesAPI.Services.Persons;

public class PersonsServices : IPersonsServices
{
    private readonly IMapper _mapperFromDto;

    private readonly IMapper _mapperToDto;

    private IPersonsRepository _repository;
    public ILogger<EmployeesController> Logger { get; set; }

    public PersonsServices(Mapper mapperFromDto, PersonsRepository repository, Mapper mapperToDto)
    {
        _mapperFromDto = mapperFromDto;

        _repository = repository;

        _mapperToDto = mapperToDto;
    }

    public bool Add(PersonDTO dto, string password)
    {
        try
        {
            Logger.LogInformation("Adding a new person to database.");

            var person = _mapperFromDto.Map<Person>(dto);

            person.Password = Encrypt.Password(password);

            bool result = _repository.Add(person);

            Logger.LogInformation("New person was added.");

            return result;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            return false;
        }
    }

    public bool Update(int id, PersonDTO dto)
    {
        try
        {
            Logger.LogInformation($"Updating person {id}");

            var person = _mapperFromDto.Map<Person>(dto);

            Logger.LogInformation($"Persons {id} updated.");

            return _repository.Update(id, person);
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());
            return false;
        }
    }

    public bool TryFind(int id, out PersonDTO dto)
    {
        try
        {
            Logger.LogInformation($"Trying to find person {id}");

            bool result = _repository.TryFind(id, out Person person);

            if (result)
            {
                Logger.LogInformation($"Person {id} found.");

                dto = _mapperToDto.Map<PersonDTO>(person);

                return result;
            }

            Logger.LogInformation($"Person {id} haven't been found.");

            dto = new PersonDTO();

            return false;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            dto = new PersonDTO();

            return false;
        }
    }

    public bool TryFind(string lastName, out PersonDTO dto)
    {
        try
        {
            Logger.LogInformation($"Trying to find person {lastName}");

            bool result = _repository.TryFind(lastName, out Person person);

            if (result)
            {
                Logger.LogInformation($"Person {lastName} found.");

                dto = _mapperToDto.Map<PersonDTO>(person);

                return result;
            }

            Logger.LogInformation($"Person {lastName} haven't been found.");

            dto = new PersonDTO();

            return false;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            dto = new PersonDTO();

            return false;
        }
    }

    public bool Delete(int id)
    {
        try
        {
            Logger.LogInformation($"Trying to delete person {id}.");

            bool result = _repository.Delete(id);

            if (result)
            {
                Logger.LogInformation($"Person {id} successfully deleted");

                return result;
            }

            Logger.LogInformation($"Person {id} haven't been deleted");

            return false;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());
            return false;
        }
    }

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<PersonDTO> dtos)
    {
        try
        {
            Logger.LogInformation($"Trying to find persons starting from id {startIndex} to id {endIndex}");

            bool result = _repository.FindRange(startIndex, endIndex, out IReadOnlyCollection<Person> persons);

            if (result)
            {
                List<PersonDTO> list = new List<PersonDTO>();

                foreach (var person in persons)
                    list.Add(_mapperToDto.Map<PersonDTO>(person));

                dtos = list;

                return result;
            }
            else
            {
                dtos = new List<PersonDTO>();

                return result;
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            dtos = new List<PersonDTO>();

            return false;
        }
    }
}