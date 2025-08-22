namespace Application.DTOs.Identity
{
    public class ApplicationUserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }

        public string? AboutMe { get; set; }
        public string? Address { get; set; }
        public string? ImagePath { get; set; }

        public float Rating { get; set; }
    }
}
