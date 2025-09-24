using LinkShortener.Entities;
using LinkShortener.Services;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
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