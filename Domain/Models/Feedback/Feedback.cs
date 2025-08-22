using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Feedback
{
    public class Feedback
    {
        [Key] public int Id { get; set; }

        [Required] public string Text { get; set; } = string.Empty;
    }
}