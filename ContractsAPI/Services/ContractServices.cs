using ContractsAPI.Dtos;
using ContractsAPI.Entities;
using ContractsAPI.Repositories;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;

namespace ContractsAPI.Services;

public class ContractServices : IContractsServices
{
    private readonly ContractRepository _repository;

    public ContractServices(ContractRepository repository)
    {
        _repository = repository;
    }

    public bool Get(HttpContext context, DateTime creationDate, int companyInn)
    {
        try
        {
            if (_repository.Get(creationDate, companyInn, out Contract contract))
            {
                if (ContractDirectories.TryFindContract(contract.CompanyInn, contract.Id, contract.CreationDate,
                        out string path))
                {
                    IFileInfo fileInfo = new PhysicalFileInfo(new FileInfo(path));
                
                    ContractFile.Send(context.Response, fileInfo);

                    return true;
                }
            
                return false;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            //todo Logger

            return false;
        }
        
        
    }

    public bool Add(HttpContext context, ContractDto contractDto, out int id)
    {
        try
        {
            if (!ContractFile.Validate(context.Request.Form.Files))
            {
                id = -1;
                return false;
            }

            Contract contract = new Contract
            {
                Id = contractDto.Id,
                CompanyInn = contractDto.CompanyInn,
                CreationDate = contractDto.CreationDate,
                LastUpdateDate = contractDto.CreationDate,
                EmployeesGroup = contractDto.EmployeesGroup,
                isDone = false
            };

            if (_repository.Add(contract, out id))
            {
                ContractDirectories.CreateCompanyDirectory(contract.CompanyInn);

                string path = ContractDirectories.CreateContractDirectory(contract.CompanyInn, id);

                var contractFile = context.Request.Form.Files[0];

                ContractFile.Copy(path, contractFile, contract.CreationDate);

                return true;
            }
            else
            {
                //todo Logger
                return false;
            }
        }
        catch (Exception e)
        {
            //todo Logger

            id = -1;
            return false;
        }
        
        
    }

    public bool Update(HttpContext context, DateTime creationDate, int companyInn)
    {
        throw new NotImplementedException();
    }

    public bool Delete(HttpContext context, DateTime creationDate, int companyInn)
    {
        throw new NotImplementedException();
    }
}