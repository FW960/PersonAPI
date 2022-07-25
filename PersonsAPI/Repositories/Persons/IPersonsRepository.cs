using PersonsAPI.Entities;

namespace PersonsAPI.Repositories.Persons;

public interface IPersonsRepository
{
    public bool Add(Person person);

    public bool Update(int id, Person newCompany);

    public bool TryFind(int id, out Person person);

    public bool TryFind(string lastName, out Person person);

    public bool Delete(int id);

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<Person> persons);
}