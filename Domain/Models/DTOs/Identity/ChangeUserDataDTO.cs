using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Models.DTOs.Identity
{
    public class ChangeUserDataDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }

        [AllowNull]
        [MinLength(3, ErrorMessage = "First name must be at least 3 characters long.")]
        public string? FirstName { get; set; }
        [AllowNull]
        [MinLength(3, ErrorMessage = "Middle name must be at least 3 characters long.")]
        public string? MiddleName { get; set; }
        [AllowNull]
        [MinLength(3, ErrorMessage = "Last name must be at least 3 characters long.")]
        public string? LastName { get; set; }

        [AllowNull]
        public DateOnly? DateOfBirth { get; set; }

        [AllowNull]
        [MinLength(3, ErrorMessage = "About me must be at least 3 characters long.")]
        public string? AboutMe { get; set; }
    }
}
