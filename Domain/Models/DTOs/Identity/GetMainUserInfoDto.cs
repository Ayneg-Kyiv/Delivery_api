using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.Identity
{
    public class GetMainUserInfoDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

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

        [MaxLength(512)]
        [Description("Driver license image to verify user, not seen by other")]
        public string? DriverLicenseImagePath { get; set; }


        [Description("User's rating, can be used for various purposes like feedback or reputation")]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public float Rating { get; set; } = 0;

    }
}
