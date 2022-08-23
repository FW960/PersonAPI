using EmployeesAPI.DTOs;

namespace EmployeesAPI.Services.Interfaces;

public interface ICompanyServices
{
    public bool Add(CompanyDto company);

    public bool Update(CompanyDto companyDto);

    public bool TryFind(int inn, out CompanyDto companyDto);

    public bool Delete(int inn);
    
    
}