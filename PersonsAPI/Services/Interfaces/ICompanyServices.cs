using EmployeesAPI.DTOs;

namespace EmployeesAPI.Services.Interfaces;

public interface ICompanyServices
{
    public bool Add(CompanyDto company);

    public bool Update(CompanyDto companyDto, int ceoId);

    public bool TryFind(string inn, out CompanyDto companyDto);

    public bool Delete(string inn);
    
    
}