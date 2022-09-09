using ContractsAPI.Dto;
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

    public bool Get(HttpContext context, int companyInn, int id)
    {
        try
        {
            if (_repository.Get(companyInn, id, out ContractDto contract))
            {
                if (ContractFile.TryFindContractFilePath(contract.CompanyInn, contract.Id, out string path))
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

    public bool Add(HttpContext context, ContractDto contract, out int id)
    {
        try
        {
            if (!ContractFile.Validate(context.Request.Form.Files))
            {
                id = -1;
                return false;
            }

            if (_repository.Add(contract, out id))
            {
                ContractDirectories.CreateCompanyDirectory(contract.CompanyInn);

                string path = ContractDirectories.CreateContractDirectory(contract.CompanyInn, id);

                var contractFile = context.Request.Form.Files[0];

                ContractFile.Copy(path, contractFile, contract.CreationDate);

                ContractFile.Send(context.Response, new PhysicalFileInfo(new FileInfo(path)));

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

    public bool Update(HttpContext context, ContractDto dto)
    {
        try
        {
            if (ContractFile.Validate(context.Request.Form.Files))
            {
                if (_repository.Update(dto))
                {
                    string directoryPath = ContractDirectories.FindContractDirectory(dto.CompanyInn, dto.Id);

                    var file = context.Request.Form.Files[0];

                    if (ContractFile.TryFindContractFilePath(dto.CompanyInn, dto.Id, out string path))
                    {
                        ContractFile.Delete(path);
                    }

                    ContractFile.Copy(directoryPath, file, dto.CreationDate);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public bool Delete(HttpContext context, ContractDto contract)
    {
        try
        {
            if (_repository.Delete(contract.CreationDate, contract.CompanyInn, contract.Id))
            {
                if (ContractFile.TryFindContractFilePath(contract.CompanyInn, contract.Id, out string path))
                {
                    ContractFile.Delete(path);
                }

                return true;
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
}