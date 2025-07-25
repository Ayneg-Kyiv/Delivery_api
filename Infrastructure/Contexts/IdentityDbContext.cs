using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
    {
        public DbSet<SessionData> Sessions { get; set; } = null!;
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

    // SessionData
    builder.Entity<SessionData>()
        .HasIndex(s => s.RefreshToken)
        .IsUnique();

    // 🚗 Vehicle → ApplicationUser
    builder.Entity<Vehicle>()
        .HasOne(v => v.Owner)
        .WithMany(u => u.Vehicles)
        .HasForeignKey(v => v.OwnerId)
        .OnDelete(DeleteBehavior.Cascade);

    // 📦 ShippingOrder → Customer (ApplicationUser)
    builder.Entity<ShippingOrder>()
        .HasOne(o => o.Customer)
        .WithMany()
        .HasForeignKey(o => o.CustomerId)
        .OnDelete(DeleteBehavior.Restrict);

    // 📬 ShippingOffer → ShippingOrder
    builder.Entity<ShippingOffer>()
        .HasOne(o => o.ShippingOrder)
        .WithMany(so => so.Offers)
        .HasForeignKey(o => o.ShippingOrderId)
        .OnDelete(DeleteBehavior.Cascade);

    // 📬 ShippingOffer → Courier (ApplicationUser)
    builder.Entity<ShippingOffer>()
        .HasOne(o => o.Courier)
        .WithMany()
        .HasForeignKey(o => o.CourierId)
        .OnDelete(DeleteBehavior.Restrict);

    // 📦 ShippingObject → ShippingOrder
    builder.Entity<ShippingObject>()
        .HasOne(o => o.ShippingOrder)
        .WithMany(order => order.Objects)
        .HasForeignKey(o => o.ShippingOrderId)
        .OnDelete(DeleteBehavior.Cascade);

    // 🚚 ShippingStartingPoint → ShippingOrder
    builder.Entity<ShippingStartingPoint>()
        .HasOne(p => p.ShippingOrder)
        .WithOne()
        .HasForeignKey<ShippingStartingPoint>(p => p.ShippingOrderId)
        .OnDelete(DeleteBehavior.Cascade);

    // 🏁 ShippingDestination → ShippingOrder
    builder.Entity<ShippingDestination>()
        .HasOne(p => p.ShippingOrder)
        .WithOne()
        .HasForeignKey<ShippingDestination>(p => p.ShippingOrderId)
        .OnDelete(DeleteBehavior.Cascade);

    // ⭐ Review → ApplicationUser
    builder.Entity<Review>()
        .HasOne(r => r.User)
        .WithMany()
        .HasForeignKey(r => r.UserId)
        .OnDelete(DeleteBehavior.Restrict);

    // ⭐ Review → ShippingOrder
    builder.Entity<Review>()
        .HasOne(r => r.ShippingOrder)
        .WithMany()
        .HasForeignKey(r => r.ShippingOrderId)
        .OnDelete(DeleteBehavior.Cascade);

    // 💬 Message → Sender
    builder.Entity<Message>()
        .HasOne(m => m.Sender)
        .WithMany()
        .HasForeignKey(m => m.SenderId)
        .OnDelete(DeleteBehavior.Restrict);

    // 💬 Message → Receiver
    builder.Entity<Message>()
        .HasOne(m => m.Receiver)
        .WithMany()
        .HasForeignKey(m => m.ReceiverId)
        .OnDelete(DeleteBehavior.Restrict);

    // 💬 Message → ShippingOrder
    builder.Entity<Message>()
        .HasOne(m => m.ShippingOrder)
        .WithMany()
        .HasForeignKey(m => m.ShippingOrderId)
        .OnDelete(DeleteBehavior.Cascade);
}

    }
}
