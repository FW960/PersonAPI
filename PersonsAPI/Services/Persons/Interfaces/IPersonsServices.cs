using EmployeesAPI.DTOs;

namespace EmployeesAPI.Services.Persons.Interfaces;

public interface IPersonsServices <T> where T: BasePersonDTO
{
    public bool TryFind(int id, out T employeeDto);

    public bool TryFind(string firstName, string lastName, out T employeeDto);

    public bool Delete(int id);

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<T> employeeDtos);
    
}