using ContractsAPI.Entities;

namespace ContractsAPI.Repositories;

public interface IContractsRepository
{
    public bool Add(Contract contract, out int id);

    public bool Update(Contract contract);

    public bool Get(DateTime creationDate, int companyInn, out Contract contract);

    public bool Delete(DateTime creationDate, int companyInn);
}