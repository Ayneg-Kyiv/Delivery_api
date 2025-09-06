using Domain.Models.Ride;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts.Extensions
{
    public static class TripEntityExtension
    {
        public static ModelBuilder SetTripEntityExtension(this ModelBuilder builder)
        {
            builder.Entity<Trip>()
                .HasMany(t => t.Slots)
                .WithOne(s => s.Trip)
                .HasForeignKey(s => s.TripId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Trip>()
                .HasMany(t => t.Orders)
                .WithOne(o => o.Trip)
                .HasForeignKey(o => o.TripId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Trip>()
                .HasOne(t => t.StartLocation)
                .WithMany(l => l.TripsStart)
                .HasForeignKey(f => f.StartLocationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Trip>()
                .HasOne(t => t.EndLocation)
                .WithMany(l => l.TripsEnd)
                .HasForeignKey(f => f.EndLocationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DeliveryOrder>()
                .HasOne(o => o.StartLocation)
                .WithMany(l => l.DeliveryOrderStartLocations)
                .HasForeignKey(o => o.StartLocationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DeliveryOrder>()
                .HasOne(o => o.EndLocation)
                .WithMany(l => l.DeliveryOrderEndLocations)
                .HasForeignKey(o => o.EndLocationId)
                .OnDelete(DeleteBehavior.NoAction);

            return builder;
        }
    }
}
