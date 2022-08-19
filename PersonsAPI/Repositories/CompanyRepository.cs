using EmployeesAPI.Entities;
using EmployeesAPI.Repositories.Interfaces;

namespace EmployeesAPI.Repositories;

public class CompanyRepository : ICompanyRepository
{
    public bool Add(Company company)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public bool TryFind(int id, out Company company)
    {
        throw new NotImplementedException();
    }

    public bool TryFind(out Company company, int inn)
    {
        throw new NotImplementedException();
    }

    public bool Update(Company company)
    {
        throw new NotImplementedException();
    }
}