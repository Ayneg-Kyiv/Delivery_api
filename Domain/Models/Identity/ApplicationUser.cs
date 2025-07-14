using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Identity
{
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
    }
}
