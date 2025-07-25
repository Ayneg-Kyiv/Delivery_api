using Domain.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Vehicles
{
    public class Vehicle
    {
        [Key] public int Id { get; set; }

        [MaxLength(64)] public string? Brand { get; set; }
        [MaxLength(64)] public string? Model { get; set; }

        [Required] public string Type { get; set; } = string.Empty;

        [Required] public Guid OwnerId { get; set; }
        [ForeignKey("OwnerId")] public ApplicationUser Owner { get; set; } = null!;

        [MaxLength(20)] public string NumberPlate { get; set; } = string.Empty;
        [MaxLength(64)] public string Color { get; set; } = string.Empty;
        [MaxLength(512)] public string? ImagePath { get; set; }
    }
}