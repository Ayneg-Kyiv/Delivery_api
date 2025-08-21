using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.Identity
{
    public class SignupDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = null!;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateOnly? BirthDate { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
