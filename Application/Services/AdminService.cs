using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models.Abstract;
using Domain.Models.DTOs;
using Domain.Models.DTOs.Vehicles;
using Domain.Models.Identity;
using Domain.Models.Ride;
using Domain.Models.Vehicles;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class AdminService(UserManager<ApplicationUser> userManager,
                              IMapper mapper,
                              IBaseRepository<Trip, ShippingDbContext> tripRepository,
                              IBaseRepository<Vehicle, ShippingDbContext> vehicleRepository,
                              IBaseRepository<DriverApplication, ShippingDbContext> driverApplicationRepository) : IAdminService
    {
        public async Task<TResponse> ApproveDriverApplicationAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            if(applicationId == Guid.Empty)
                return TResponse.Failure(400, "Invalid application ID.");

            var application = await driverApplicationRepository.FindAsync([a => a.Id == applicationId], cancellationToken);

            if(application.First() == null)
                return TResponse.Failure(404, "Driver application not found.");

            var user = await userManager.FindByIdAsync(application.First().UserId.ToString());

            if(user == null)
                return TResponse.Failure(404, "User associated with the application not found.");

            var vehicle = await vehicleRepository.FindAsync([v => v.Id == application.First().VehicleId], cancellationToken);

            if(vehicle.First() == null)
                return TResponse.Failure(404, "Vehicle associated with the application not found.");

            var addToRoleResult = await userManager.AddToRoleAsync(user, "Driver");

            if(!addToRoleResult.Succeeded)
                return TResponse.Failure(500, "Failed to assign Driver role to the user.");

            vehicle.First().IsApproved = true;

            var updateVehicleResult = await vehicleRepository.UpdateAsync(vehicle.First(), cancellationToken);

            if(!updateVehicleResult)
                return TResponse.Failure(500, "Failed to approve the vehicle.");

            var deleteApplicationResult = await driverApplicationRepository.DeleteAsync(application.First(), cancellationToken);

            if(!deleteApplicationResult)
                return TResponse.Failure(500, "Failed to delete the driver application.");

            return TResponse.Successful(message: "Driver application approved successfully.");
        }

        public async Task<TResponse> GetAdminPanelDataAsync()
        {
            var usersCount = userManager.Users.Count();

            var drivers = await userManager.GetUsersInRoleAsync("Driver");
            var driversCount = drivers.Count;

            var totalTrips = await tripRepository.GetTotalCountAsync([t => !t.IsCompleted], default);

            var data = new
            {
                UsersCount = usersCount,
                DriversCount = driversCount,
                TotalTrips = totalTrips
            };

            return TResponse.Successful(data, "Admin panel data retrieved successfully.");
        }

        public async Task<TResponse> GetDriversApplicationAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            if(pageNumber <= 0 || pageSize <= 0)
                return TResponse.Failure(400, "Page number and page size must be greater than zero.");

            var totalApplications = await driverApplicationRepository.GetTotalCountAsync([], cancellationToken);

            var applications = await driverApplicationRepository.FindWithIncludesAndPaginationAsync(
                [],
                pageNumber,
                pageSize,
                cancellationToken,
                [a => a.Vehicle]);

            var applicationsDto = mapper.Map<List<GetDriverApplicationDto>>(applications);

            var pagination = new Pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalApplications
            };

            var response = new PaginatedPage
            {
                Data = applicationsDto,
                Pagination = pagination
            };

            return TResponse.Successful(response, "Driver applications retrieved successfully.");
        }

        public async Task<TResponse> RejectDriverApplicationAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            if (applicationId == Guid.Empty)
                return TResponse.Failure(400, "Invalid application ID.");

            var application = await driverApplicationRepository.FindAsync([a => a.Id == applicationId], cancellationToken);

            if (application.First() == null)
                return TResponse.Failure(404, "Driver application not found.");

            var user = await userManager.FindByIdAsync(application.First().UserId.ToString());

            if (user == null)
                return TResponse.Failure(404, "User associated with the application not found.");

            var vehicle = await vehicleRepository.FindAsync([v => v.Id == application.First().VehicleId], cancellationToken);

            if (vehicle.First() == null)
                return TResponse.Failure(404, "Vehicle associated with the application not found.");

            var deleteVehicleResult = await vehicleRepository.DeleteAsync(vehicle.First(), cancellationToken);

            if (!deleteVehicleResult)
                return TResponse.Failure(500, "Failed to delete the vehicle.");

            return TResponse.Successful("Driver application rejected and vehicle deleted successfully.");
        }
    }
}
