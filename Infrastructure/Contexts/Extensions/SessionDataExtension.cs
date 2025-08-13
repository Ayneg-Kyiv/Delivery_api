using Domain.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts.Extensions
{
    public static class SessionDataExtension
    {
        public static ModelBuilder SetSessionDataExtension(this ModelBuilder builder)
        {
            builder.Entity<SessionData>()
                .HasIndex(s => s.RefreshToken)
                .IsUnique();

            builder.Entity<SessionData>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}
