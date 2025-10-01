using Domain.Models.Feedback;
using Domain.Models.Messaging;
using Domain.Models.News;
using Domain.Models.Orders;
using Domain.Models.Reviews;
using Domain.Models.Ride;
using Domain.Models.Vehicles;
using Infrastructure.Contexts.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts
{
    public class ShippingDbContext(DbContextOptions<ShippingDbContext> options) 
        : DbContext(options)
    {
        // Vehicles and Driver Applications
        public virtual DbSet<Vehicle> Vehicles { get; set; } = null!;
        public virtual DbSet<DriverApplication> DriverApplications { get; set; } = null!;

        //public virtual DbSet<ShippingOrder> ShippingOrders { get; set; } = null!;
        //public virtual DbSet<ShippingOffer> ShippingOffers { get; set; } = null!;
        //public virtual DbSet<ShippingObject> ShippingObjects { get; set; } = null!;
        //public virtual DbSet<ShippingStartingPoint> ShippingStartingPoints { get; set; } = null!;
        //public virtual DbSet<ShippingDestination> ShippingDestinations { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;

        //News and Articles
        public virtual DbSet<Article> Articles { get; set; } = null!;
        public virtual DbSet<ArticleBlock> ArticleBlocks { get; set; } = null!;

        // Trips
        public virtual DbSet<Trip> Trips { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<DeliverySlot> DeliverySlots { get; set; } = null!;
        public virtual DbSet<DeliveryOrder> DeliveryOrders { get; set; } = null!;
        public virtual DbSet<DeliveryRequest> DeliveryRequests { get; set; } = null!;
        public virtual DbSet<DeliveryOffer> DeliveryOffers { get; set; } = null!;

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

            builder.SetTripEntityExtension();

            builder.ConfigureArticle();
        }
    }
}