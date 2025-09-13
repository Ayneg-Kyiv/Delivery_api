using Domain.Models.DTOs;

namespace Domain.Interfaces.Services
{
    public interface IAdminService
    {
        Task<TResponse> GetAdminPanelDataAsync();
        Task<TResponse> GetDriversApplicationAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);
        Task<TResponse> ApproveDriverApplicationAsync(
            Guid applicationId,
            CancellationToken cancellationToken);
        Task<TResponse> RejectDriverApplicationAsync(
            Guid applicationId,
            CancellationToken cancellationToken);

    }
}
