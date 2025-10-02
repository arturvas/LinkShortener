using LinkShortener.API;
using LinkShortener.API.Entities;
using LinkShortener.API.Extensions;
using LinkShortener.API.Models;
using LinkShortener.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlServer(connString));

builder.Services.AddScoped<UrlShorteningService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.ApplyMigrations();
}

app.MapPost("api/shorten", async (
    ShortenUrlRequest request,
    UrlShorteningService service,
    ApplicationDbContext dbContext,
    HttpContext httpContext) =>
{
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("The specified URL is not valid.");
    }
    
    var code = await service.GenerateUniqueCode();

    var shortenedUrl = new ShortenedUrl
    {
        Id = Guid.NewGuid(),
        LongUrl = request.Url,
        Code = code,
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
        CreatedAt = DateTimeOffset.UtcNow
    };
    
    dbContext.ShortenedUrls.Add(shortenedUrl);
    
    await dbContext.SaveChangesAsync();
    
    return Results.Ok(shortenedUrl.ShortUrl);
});

app.MapGet("api/{code}", async (string code, ApplicationDbContext dbContext) =>
    {
        var shortenedUrl = await dbContext.ShortenedUrls.FirstOrDefaultAsync(s => s.Code == code);
        
        return shortenedUrl == null ? Results.NotFound() : Results.Redirect(shortenedUrl.LongUrl);
    }
);

app.MapGet("api/links", (ApplicationDbContext dbContext) => dbContext.ShortenedUrls.ToList());

app.UseHttpsRedirection();

app.Run();