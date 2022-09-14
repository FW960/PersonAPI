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

    public bool Get(HttpContext context)
    {
        try
        {
            string token = context.Request.Headers.First(x => x.Key.Equals("Authorization")).Value.First().Split("Bearer ")[1];

            string email = JwtToken.GetEmailFromClaims(token);

            if (_repository.Get(email, out ContractDto contract))
            {
                if (ContractFile.TryFindContractFilePath(contract.CompanyInn, contract.Id, out string path))
                {
                    IFileInfo fileInfo = new PhysicalFileInfo(new FileInfo(path));

                    ContractFile.Send(context.Response, fileInfo);

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
        catch
        {
            //todo logger

            return false;
        }
    }

    public bool Add(HttpContext context, int inn, int group, decimal price, out int id)
    {
        try
        {
            if (!ContractFile.Validate(context.Request.Form.Files))
            {
                id = -1;
                return false;
            }

            ContractDto contract = new ContractDto
            {
                CompanyInn = inn,
                EmployeesGroup = group,
                CreationDate = DateTime.Now,
                Price = price
            };

            if (_repository.Add(contract, out id))
            {
                ContractDirectories.CreateCompanyDirectory(contract.CompanyInn);

                string path = ContractDirectories.CreateContractDirectory(contract.CompanyInn, id);

                var contractFile = context.Request.Form.Files[0];

                path = ContractFile.Copy(path, contractFile, id);

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

                    ContractFile.Copy(directoryPath, file, dto.Id);

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