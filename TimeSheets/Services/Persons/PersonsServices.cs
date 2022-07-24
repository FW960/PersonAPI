using TimeSheets.DTOs;

namespace TimeSheets.Services.Persons;

public class PersonsServices : IPersonsServices
{
    public void Add(PersonDTO person)
    {
        throw new NotImplementedException();
    }

    public void Update(string newCompany)
    {
        throw new NotImplementedException();
    }

    public bool TryFind(int id, out PersonDTO person)
    {
        throw new NotImplementedException();
    }

    public bool TryFind(string name, out PersonDTO person)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public List<PersonDTO> FindRange(int startIndex, int amount)
    {
        throw new NotImplementedException();
    }
}