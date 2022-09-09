using ContractsAPI.Dto;

namespace ContractsAPI.Services;

public interface IContractsServices
{
    public bool Get(HttpContext context, int companyInn, int id);

    public bool Add(HttpContext context, ContractDto contract, out int id);

    public bool Update(HttpContext context, ContractDto dto);

    public bool Delete(HttpContext context, ContractDto dto);
}