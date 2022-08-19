using EmployeesAPI.DTOs;

namespace EmployeesAPI.Services.Interfaces;

public interface ICompanyServices
{
    public bool Add(CompanyDto companyDto);

    public bool Update(CompanyDto companyDto);

    public bool Delete(int id);

    public bool TryFind(int id, out CompanyDto companyDto);

    public bool TryFind(out CompanyDto companyDto, int inn);
}