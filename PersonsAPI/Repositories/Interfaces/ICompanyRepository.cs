using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;

namespace EmployeesAPI.Repositories.Interfaces;

public interface ICompanyRepository
{
    public bool Add(Company company);

    public bool Delete(int id);

    public bool TryFind(int inn, out Company company);

    public bool Update(Company company);
}