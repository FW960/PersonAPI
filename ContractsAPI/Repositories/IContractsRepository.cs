namespace ContractsAPI.Repositories;

public interface IContractsRepository
{
    public bool Add(string name);

    public bool Update(string name);

    public bool Get(string name);

    public bool Delete(string name);
}