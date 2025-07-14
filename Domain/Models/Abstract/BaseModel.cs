using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Abstract
{
    public abstract class BaseModel
    {
        [Key]
        public Guid Id { get; private set; }
        public void SetId(Guid id) => Id = id;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public void SetCreatedAt() => CreatedAt = DateTime.UtcNow;

        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public void SetUpdatedAt() => UpdatedAt = DateTime.UtcNow;
    }
}
