using Domain.Models.Abstract;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Domain.Models.Identity
{
    public class SessionData : BaseModel
    {
        [Required]
        public Guid UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        [Required]
        public string RefreshToken { get; set; } = string.Empty;

        public string? DeviceIdentifier { get; set; }

        public string? UserAgent { get; set; }

        public IPAddress? IpAddress { get; private set; }
        public void SetIpAddress(IPAddress ipAddress) => IpAddress = ipAddress;

        public string? Country { get; set; }

        public DateTime FirstActive { get; init; } = DateTime.UtcNow;

        public DateTime LastActive { get; private set; } = DateTime.UtcNow;
        public void SetLastActive() => LastActive = DateTime.UtcNow;
    }
}
