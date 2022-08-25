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

    public static void Copy(string path, IFormFile contractFile, DateTime creationDate)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);

        contractFile.CopyTo(fileStream);
        
        fileStream.Close();

        File.Move(path + $@"\{contractFile.Name}", path + $@"\{creationDate}");
    }

    public static void Send(HttpResponse response, IFileInfo fileInfo)
    {
        response.SendFileAsync(fileInfo, CancellationToken.None);
    }
}