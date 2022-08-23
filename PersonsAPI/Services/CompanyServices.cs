using AutoMapper;
using EmployeesAPI.Controllers;
using EmployeesAPI.DTOs;
using EmployeesAPI.Entities;
using EmployeesAPI.Repositories;
using EmployeesAPI.Services.Interfaces;
using MySqlConnector;

namespace EmployeesAPI.Services;

public class CompanyServices : ICompanyServices
{
    private readonly CompanyRepository _repository;
    public ILogger<CompaniesController> Logger { get; set; }

    public CompanyServices(CompanyRepository repository)
    {
        _repository = repository;
    }

    public bool Add(CompanyDto companyDto)
    {
        Logger.LogInformation($"Trying to add new company with inn{companyDto.Inn}");

        Company company = new Company
        {
            Inn = companyDto.Inn,
            Name = companyDto.Name,
            Ceo = new Ceo()
        };

        try
        {
            bool result = _repository.Add(company);

            Logger.LogInformation($"Company {company.Inn} have been succsesfully added");

            return result;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            return false;
        }
    }

    public bool Update(CompanyDto companyDto)
    {
        Logger.LogInformation($"Trying to update {companyDto.Inn}");

        Company company = new Company
        {
            Inn = companyDto.Inn,
            Name = companyDto.Name,
            Ceo = new Ceo()
        };

        try
        {
            bool result = _repository.Update(company);

            Logger.LogInformation($"Company {company.Inn} updated");

            return result;
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());

            return false;
        }
    }

    public bool TryFind(int inn, out CompanyDto companyDto)
    {
        Logger.LogInformation($"Trying to find company {inn}");

        try
        {
            bool result = _repository.TryFind(inn, out Company company);


            if (result)
            {
                companyDto = new CompanyDto
                {
                    Inn = inn,
                    Name = company.Name
                };
                try
                {
                    companyDto.Ceo = new CeoDto
                    {
                        Email = company.Ceo.Email,
                        FirstName = company.Ceo.FirstName,
                        LastName = company.Ceo.LastName
                    };
                }
                catch
                {
                    Logger.LogInformation($"Company {company.Inn} doesn't have CEO");
                }

                Logger.LogInformation($"Found company {inn}");
                return true;
            }
            else
            {
                companyDto = new CompanyDto();
                
                Logger.LogInformation($"Haven't found company {inn}");
                return false;
            }
        }
        catch (Exception e)
        {
            companyDto = new CompanyDto();

            Logger.LogInformation(e.ToString());
            return false;
        }
    }

    public bool Delete(int inn)
    {
        try
        {
            Logger.LogInformation($"Trying to delete company {inn}");

            bool result = _repository.Delete(inn);

            if (result)
            {
                Logger.LogInformation($"Company {inn} have been deleted");
                return result;
            }
            else
            {
                Logger.LogInformation($"Company {inn} haven't been deleted");
                return result;
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e.ToString());
            return false;
        }
    }
}