using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.Identity
{
    public class UpdateProfileImageDTO
    {
        [Required]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Image file to be uploaded as the new profile picture.
        /// </summary>
        [Required]
        [DataType(DataType.Upload)]
        [SwaggerSchema(Format = "binary", Description = "The user's profile featured image")]
        public IFormFile Image { get; set; } = null!;
    }
}
