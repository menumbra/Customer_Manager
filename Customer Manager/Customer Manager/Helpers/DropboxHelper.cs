using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Customer_Manager.Helpers;

public static class DropboxHelper
{
    private const string AccessToken = "YOUR_DROPBOX_ACCESS_TOKEN_HERE";

    public static async Task UploadFolderAsync(string localFolderPath, string dropboxFolderPath)
    {
        using var dbx = new DropboxClient(AccessToken);

        if (!Directory.Exists(localFolderPath))
        {
            throw new DirectoryNotFoundException($"Local folder not found: {localFolderPath}");
        }

        var files = Directory.GetFiles(localFolderPath, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var relativePath = Path.GetRelativePath(localFolderPath, file).Replace("\\", "/");
            var dropboxPath = $"{dropboxFolderPath}/{relativePath}";

            using var fileStream = File.OpenRead(file);
            await dbx.Files.UploadAsync(
                path: dropboxPath,
                mode: WriteMode.Overwrite.Instance,
                autorename: false,
                body: fileStream);

        }
    }
}
