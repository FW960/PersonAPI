using Microsoft.Extensions.FileProviders;

namespace ContractsAPI.Services;

public static class ContractFile
{
    public static bool Validate(IFormFileCollection files)
    {
        if (files.Count > 1 || files.Count <= 0)
            return false;

        if (!files[0].Name.Contains(".pdf"))
            return false;

        return true;
    }

    public static string Copy(string path, IFormFile contractFile, int id)
    {
        path = path + @"\" + $"contract-{id}" + ".pdf";

        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

        contractFile.CopyTo(fileStream);

        fileStream.Close();

        return path;
    }

    public static bool Delete(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);

            return true;
        }

        return false;
    }

    public static async void Send(HttpResponse response, IFileInfo fileInfo)
    {
        response.Headers.ContentDisposition = $"attachment; filename={fileInfo.Name}";
        
        await response.SendFileAsync(fileInfo.PhysicalPath);
    }


    public static bool TryFindContractFilePath(int compannyInn, int contractId, out string path)
    {
        string directoryPath =
            $@"C:\Users\windo\RiderProjects\TimeSheets\ContractsApi\Contracts\{compannyInn}\{contractId}";

        var files = Directory.EnumerateFileSystemEntries(directoryPath);

        foreach (var file in files)
        {
            if (file.Contains("contract-"))
            {
                path = file;
                return true;
            }
        }

        path = "";
        return false;
    }
}