using PersonsAPI.DTOs;

namespace PersonsAPI.Services.Persons;

public interface IPersonsServices
{
    public bool Add(PersonDTO dto);
    
    public bool Update(int id, PersonDTO dto);
    
    public bool TryFind(int id, out PersonDTO dto);

    public bool TryFind(string lastName, out PersonDTO dto);

    public bool Delete(int id);

    public bool FindRange(int startIndex, int endIndex, out IReadOnlyCollection<PersonDTO> dtos);
    
}