using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class FileService(IWebHostEnvironment environment) : IFileService
    {
        public async Task<string> SaveFileAsync(IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
                // Generate a safe file name
                string fileName = new string([.. Path.GetFileNameWithoutExtension(file.FileName).Take(20)]).Replace(" ", "_");
                fileName += $"_{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(file.FileName)}";

                // Use IWebHostEnvironment to get the content root path
                string filesDirectory = Path.Combine(environment.ContentRootPath, "Files");

                // Ensure the Files directory exists
                if (!Directory.Exists(filesDirectory))
                    Directory.CreateDirectory(filesDirectory);

                string filePath = Path.Combine(filesDirectory, fileName);

                // Save the file
                using (var fs = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(fs, cancellationToken);

                // Return only the file name
                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
                return String.Empty;
            }
        }

        public bool DeleteFileAsync(string fileName)
        {
            try
            {
                // Use IWebHostEnvironment to get the content root path
                string filesDirectory = Path.Combine(environment.ContentRootPath, "Files");
                string filePath = Path.Combine(filesDirectory, fileName);

                if (!File.Exists(filePath))
                    return false;

                File.Delete(filePath);
                return true;
            }
            catch
            {
                Console.WriteLine($"Error deleting file: {fileName}");
                return false;
            }
        }
    }
}
