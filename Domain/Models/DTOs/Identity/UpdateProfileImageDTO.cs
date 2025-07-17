using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.Identity
{
    public class UpdateProfileImageDTO
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public IFormFile Image { get; set; } = null!;
    }
}
