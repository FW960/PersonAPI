using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;

namespace EmployeesAPI.Repositories.Interfaces;

public interface ICompanyRepository
{
    public bool Add(Company company);

    public bool Delete(string id);

    public bool TryFind(string inn, out Company company);

    public bool Update(Company company);
}