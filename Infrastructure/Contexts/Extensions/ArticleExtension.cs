using Domain.Models.News;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;

namespace Infrastructure.Contexts.Extensions
{
    public static class ArticleExtension
    {
        public static void ConfigureArticle(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasMany(a => a.ArticleBlocks)
                .WithOne(ab => ab.Article)
                .HasForeignKey(ab => ab.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
