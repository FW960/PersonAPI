using Microsoft.AspNetCore.Mvc;

namespace ContractsAPI.Services;

public static class ContractDirectories
{
    public static string CreateContractDirectory(string companyInn, int contractId)
    {
        string path = $@"C:\Users\windo\RiderProjects\TimeSheets\ContractsAPI\Contracts\{companyInn}\{contractId}";

        if (Directory.Exists(path))
            return path;

        Directory.CreateDirectory(path);

        return path;
    }

    public static void CreateCompanyDirectory(string companyInn)
    {
        string path = $@"C:\Users\windo\RiderProjects\TimeSheets\ContractsAPI\Contracts\{companyInn}";
        
        if (Directory.Exists(path))
            return;

        Directory.CreateDirectory(path);
    }

    public static bool TryFindContract(string compannyInn, int contractId, DateTime creationDate, out string path)
    {
        path =
            $@"C:\Users\windo\RiderProjects\TimeSheets\ContractsApi\Contracts\{compannyInn}\{contractId}\{creationDate}";

        if (File.Exists(path))
            return true;
        
        return false;
    }
}