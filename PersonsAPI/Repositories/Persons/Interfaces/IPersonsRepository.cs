using EmployeesAPI.Entities;

namespace EmployeesAPI.Repositories.Persons.Interfaces;

public interface IPersonsRepository<T> where T : BasePersonEntity
{
    public bool Add(T employee);

    public bool Update(T employee);

    public bool TryFind(int id, out T employee);

    public bool TryFind(string firstName, string lastName, out T employee);

    public bool Delete(int id);

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<T> employee);
}