using TimeSheets.Entities;

namespace TimeSheets.Repositories.Persons;

public class PersonRepository : IPersonsRepository
{
    public void Add(Person person)
    {
        throw new NotImplementedException();
    }

    public void Update(string newCompany)
    {
        throw new NotImplementedException();
    }

    public bool TryFind(int id, out Person person)
    {
        throw new NotImplementedException();
    }

    public bool TryFind(string name, out Person person)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public List<Person> FindRange(int startIndex, int amount)
    {
        throw new NotImplementedException();
    }
}