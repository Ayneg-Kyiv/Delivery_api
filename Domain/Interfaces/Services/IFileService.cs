using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, CancellationToken cancellationToken);
        bool DeleteFileAsync(string fileName);
    }
}
