namespace Domain.Models.DTOs.Ride.Location
{
    public class GetLocationDto
    {
        public Guid Id { get; set; }
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
