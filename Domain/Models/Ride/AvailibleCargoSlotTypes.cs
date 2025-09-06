namespace Domain.Models.Ride
{
    public class AvailableCargoSlotTypes
    {
        public static Dictionary<string, (string MaxWeight, string MaxVolume)> Types { get; } = new()
        {
            { "XS", ("1kg", "0.5L") },
            { "S", ("3kg", "1L") },
            { "M", ("5kg", "2L") },
            { "L", ("7kg", "2L") },
            { "XL", ("10kg", "2L") },
            { "XXL", ("10kg+", "2L+") }
        };
    }
}
