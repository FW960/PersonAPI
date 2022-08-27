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

    public static string Copy(string path, IFormFile contractFile, DateTime creationDate)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);

        contractFile.CopyTo(fileStream);

        fileStream.Close();

        File.Move(path + $@"\{contractFile.Name}", path + $@"\Contract {creationDate}");

        return path + $@"\{creationDate}";
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

    public static void Send(HttpResponse response, IFileInfo fileInfo)
    {
        response.Headers.ContentDisposition = $"attachment; filename={fileInfo.Name}";

        response.SendFileAsync(fileInfo, CancellationToken.None);
    }


    public static bool TryFindContractFilePath(string compannyInn, int contractId, out string path)
    {
        string directoryPath =
            $@"C:\Users\windo\RiderProjects\TimeSheets\ContractsApi\Contracts\{compannyInn}\{contractId}";

        var files = Directory.EnumerateFileSystemEntries(directoryPath);

        foreach (var file in files)
        {
            if (file.Contains("Contract"))
            {
                path = file;
                return true;
            }
        }

        path = "";
        return false;
    }
}