using TimeSheets.DTOs;

namespace TimeSheets.Services.Persons;

public interface IPersonsServices
{
    public void Add(PersonDTO person);
    
    public void Update(string newCompany);
    
    public bool TryFind(int id, out PersonDTO person);

    public bool TryFind(string name, out PersonDTO person);

    public bool Delete(int id);

    public List<PersonDTO> FindRange(int startIndex, int amount);
    
}