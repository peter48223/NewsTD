using Microsoft.EntityFrameworkCore;

namespace NewsTD
{
    public class ArticleDbContext : DbContext
    {
        public ArticleDbContext(DbContextOptions<ArticleDbContext> options) : base(options)
        {

        }

        public DbSet<Article> Article { get; set; }
        public DbSet<Source> Source { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>().OwnsOne(p => p.Source);
        }

    }
}
