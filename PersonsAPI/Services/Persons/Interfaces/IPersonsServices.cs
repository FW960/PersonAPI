using EmployeesAPI.DTOs;

namespace EmployeesAPI.Services.Persons.Interfaces;

public interface IPersonsServices <T> where T: BasePersonDTO
{
    public bool Update(int id, T personDto);
    
    public bool TryFind(int id, out T personDto);

    public bool TryFind(string firstName, string lastName, out T personDto);

    public bool Delete(int id);

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<T> dtos);
    
}