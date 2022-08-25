using EmployeesAPI.Entities;

namespace EmployeesAPI.Repositories.Persons.Interfaces;

public interface IPersonsRepository<T> where T : BasePersonEntity
{
    public bool Add(T employee);

    public bool Update(int id, T employee);

    public bool TryFind(int id, out T personEntity);

    public bool TryFind(string firstName, string lastName, out T personEntity);

    public bool Delete(int id);

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<T> persons);
}