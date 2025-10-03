using Azure.Storage.Blobs;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class BlobService(IConfiguration configuration) : IFileService
    {
        public bool DeleteFileAsync(string fileName)
        {
            try
            {
                var blobStorage = new BlobServiceClient(configuration.GetSection("BlobStorage").GetValue<string>("String"));
                var blobContainer = blobStorage.GetBlobContainerClient("Files");
                var blobClient = blobContainer.GetBlobClient(fileName);
                return blobClient.DeleteIfExists();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
                string fileName = new string(Path.GetFileNameWithoutExtension(file.FileName).Take(20).ToArray()).Replace(" ", "_");
                fileName += $"{DateTime.Now:yymmssfff}{Path.GetExtension(file.FileName)}";

                var blobStorage = new BlobServiceClient(configuration.GetSection("BlobStorage").GetValue<string>("String"));
                var blobContainer = blobStorage.GetBlobContainerClient("files");

                using var memoryStream = new MemoryStream();

                await file.CopyToAsync(memoryStream, cancellationToken);
                memoryStream.Position = 0;

                await blobContainer.UploadBlobAsync(fileName, memoryStream, default);

                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
    }
}
