namespace Domain.Models.DTOs.Identity
{
    public class GetApplicationUserForTripDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public float Rating { get; set; }
    }
}
