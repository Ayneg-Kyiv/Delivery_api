using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTO.Identity
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Current password is required.")]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "New password is required.")]
        [MinLength(8, ErrorMessage = "New password must be at least 6 characters long.")]
        public string NewPassword { get; set; } = null!;
    }
}
