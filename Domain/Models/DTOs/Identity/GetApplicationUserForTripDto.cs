using Domain.Models.DTOs.Reviews;

namespace Domain.Models.DTOs.Identity
{
    public class GetApplicationUserForTripDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public float Rating { get; set; }
        public ICollection<ReviewDto>? Reviews { get; set; } = null!;
    }
}
