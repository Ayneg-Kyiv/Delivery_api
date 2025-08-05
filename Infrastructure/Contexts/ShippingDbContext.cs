using Domain.Models.Feedback;
using Domain.Models.Messaging;
using Domain.Models.Orders;
using Domain.Models.Reviews;
using Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using System;
namespace Infrastructure.Contexts
{
    public class ShippingDbContext : DbContext
    {
        public ShippingDbContext(DbContextOptions<ShippingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<ShippingOrder> ShippingOrders { get; set; } = null!;
        public DbSet<ShippingOffer> ShippingOffers { get; set; } = null!;
        public DbSet<ShippingObject> ShippingObjects { get; set; } = null!;
        public DbSet<ShippingStartingPoint> ShippingStartingPoints { get; set; } = null!;
        public DbSet<ShippingDestination> ShippingDestinations { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // FK configuration for ShippingDestination (One-to-Many)
            builder.Entity<ShippingDestination>()
                .HasOne(d => d.ShippingOrder)
                .WithMany(o => o.ShippingDestinations)
                .HasForeignKey(d => d.ShippingOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ShippingOffer>()
                .Property(o => o.OfferedPrice)
                .HasPrecision(18, 4);

            builder.Entity<ShippingOrder>()
                .Property(o => o.EstimatedCost)
                .HasPrecision(18, 4);
        }
    }
}