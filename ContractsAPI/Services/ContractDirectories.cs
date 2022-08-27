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

    
    public static string FindContractDirectory(string companyInn, int contractId)
    {
        string directoryPath = $@"C:\Users\windo\RiderProjects\TimeSheets\ContractsAPI\Contracts\{companyInn}\{contractId}";

        return directoryPath;
    }
    
    public static void CreateCompanyDirectory(string companyInn)
    {
        string path = $@"C:\Users\windo\RiderProjects\TimeSheets\ContractsAPI\Contracts\{companyInn}";
        
        if (Directory.Exists(path))
            return;

        Directory.CreateDirectory(path);
    }
}