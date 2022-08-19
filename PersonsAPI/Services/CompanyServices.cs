using EmployeesAPI.DTOs;
using EmployeesAPI.Services.Interfaces;

namespace EmployeesAPI.Services;

public class CompanyServices : ICompanyServices
{
    public bool Add(CompanyDto companyDto)
    {
        throw new NotImplementedException();
    }

    public bool Update(CompanyDto companyDto)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public bool TryFind(int id, out CompanyDto companyDto)
    {
        throw new NotImplementedException();
    }

    public bool TryFind(out CompanyDto companyDto, int inn)
    {
        throw new NotImplementedException();
    }
}