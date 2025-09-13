namespace Domain.Models.DTOs.Identity
{
    public class GetMainUserInfoDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string? ImagePath { get; set; }
    }
}
