using LinkShortener.API.Entities;
using LinkShortener.API.Services;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.API;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder
                .Property(shortenedUrl => shortenedUrl.Code)
                .HasMaxLength(UrlShorteningService.NumberOfCharacters);
            builder
                .HasIndex(shortenedUrl => shortenedUrl.Code)
                .IsUnique();
        });
    }
}