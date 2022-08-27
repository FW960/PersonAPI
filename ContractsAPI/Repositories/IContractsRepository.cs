using ContractsAPI.Dtos;

namespace ContractsAPI.Repositories;

public interface IContractsRepository
{
    public bool Add(ContractDto contract, out int id);

    public bool Update(ContractDto contract);

    public bool Get(string companyInn, int id, out ContractDto contract);

    public bool Delete(DateTime creationDate, string companyInn, int id);
}