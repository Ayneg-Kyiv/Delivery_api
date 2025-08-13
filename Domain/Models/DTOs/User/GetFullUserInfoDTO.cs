namespace Domain.Models.DTOs.User
{
    public class GetFullUserInfoDTO
    {
        public Guid Id { get; set; }
        
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        
        public string? AboutMe { get; set; }
        
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public float Rating { get; set; } = 0;
    }
}
