using ContractsAPI.Dto;

namespace ContractsAPI.Repositories;

public interface IContractsRepository
{
    public bool Add(ContractDto contract, out int id);

    public bool Update(ContractDto contract);

    public bool Get(int companyInn, int id, out ContractDto contract);

    public bool Delete(DateTime creationDate, int companyInn, int id);
}