using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;

namespace EmployeesAPI.Repositories.Interfaces;

public interface ICompanyRepository
{
    public bool Add(Company company);

    public bool Delete(int id);

    public bool TryFind(int id, out Company company);

    public bool TryFind(out Company company, int inn);

    public bool Update(Company company);
}