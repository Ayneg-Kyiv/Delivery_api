using Domain.Models.Feedback;
using Domain.Models.Identity;
using Domain.Models.Messaging;
using Domain.Models.Orders;
using Domain.Models.Reviews;
using Domain.Models.Vehicles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Contexts
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        public DbSet<SessionData> Sessions { get; set; } = null!;
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // SessionData
            builder.Entity<SessionData>()
                .HasIndex(s => s.RefreshToken)
                .IsUnique();

           
        }
    }
}
