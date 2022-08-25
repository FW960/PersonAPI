using ContractsAPI.Dtos;

namespace ContractsAPI.Services;

public interface IContractsServices
{
    public bool Get(HttpContext context, DateTime creationDate, int companyInn);

    public bool Add(HttpContext context, ContractDto contractDto, out int id);

    public bool Update(HttpContext context, DateTime creationDate, int companyInn);

    public bool Delete(HttpContext context, DateTime creationDate, int companyInn);
}