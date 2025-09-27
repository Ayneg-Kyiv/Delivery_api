using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.Auth
{
    public class GoogleAuthRequest
    {
        [Required]
        public string IdToken { get; set; } = null!;

        public string? AccessToken { get; set; }
    }
}