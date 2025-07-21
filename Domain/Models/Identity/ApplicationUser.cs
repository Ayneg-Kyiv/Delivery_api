using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models.Identity
{
    //Indexations are used to speed up queries on these fields
    [Index(nameof(Email))]
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? MiddleName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        [Description("Some additional information about user provided by himself")]
        [MaxLength(256)]
        public string? AboutMe { get; set; }

        [MaxLength(512)]
        [Description("User's address, can be used for shipping or billing purposes")]
        public string? Address { get; set; }

        [MaxLength(512)]
        [Description("User's profile image Path")]
        public string? ImagePath { get; set; }

        [Description("User's rating, can be used for various purposes like feedback or reputation")]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public float Rating { get; set; } = 0;

        [JsonPropertyName("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
