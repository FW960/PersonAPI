using TimeSheets.Entities;

namespace TimeSheets.Repositories.Persons;

public interface IPersonsRepository
{
    public void Add(Person person);

    public void Update(string newCompany);

    public bool TryFind(int id, out Person person);

    public bool TryFind(string name, out Person person);

    public bool Delete(int id);

    public List<Person> FindRange(int startIndex, int amount);
}