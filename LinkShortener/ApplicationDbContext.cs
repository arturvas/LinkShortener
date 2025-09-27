using LinkShortener.Entities;
using LinkShortener.Services;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.Property(s => s.Code).HasMaxLength(UrlShorteningService.NumberOfCharacters);
            builder.HasIndex(s => s.Code).IsUnique();
        });
    }
}