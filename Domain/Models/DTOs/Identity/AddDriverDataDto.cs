using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.Identity
{
    public class AddDriverDataDto
    {
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? Image { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? ProfileImage { get; set; }
    }
}
