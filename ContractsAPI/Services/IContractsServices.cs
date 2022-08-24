namespace ContractsAPI.Services;

public interface IContractsServices
{
    public bool Get(string name);

    public bool Add(HttpContext context);

    public bool Update(string name);

    public bool Delete(string name);
}