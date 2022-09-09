namespace ContractsAPI.Services;

public static class ContractDirectories
{
    public static string CreateContractDirectory(int companyInn, int contractId)
    {
        string path = $@"C:\Users\windo\RiderProjects\TimeSheets\ContractsAPI\Contracts\{companyInn}\{contractId}";

        if (Directory.Exists(path))
            return path;

        Directory.CreateDirectory(path);

        return path;
    }

    
    public static string FindContractDirectory(int companyInn, int contractId)
    {
        string directoryPath = $@"C:\Users\windo\RiderProjects\TimeSheets\ContractsAPI\Contracts\{companyInn}\{contractId}";

        return directoryPath;
    }
    
    public static void CreateCompanyDirectory(int companyInn)
    {
        string path = $@"C:\Users\windo\RiderProjects\TimeSheets\ContractsAPI\Contracts\{companyInn}";
        
        if (Directory.Exists(path))
            return;

        Directory.CreateDirectory(path);
    }
}