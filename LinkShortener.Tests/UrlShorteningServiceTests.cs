using LinkShortener.Services;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Tests;

public class UrlShorteningServiceTests
{
    [Fact]
    public async Task GenerateCode_ShouldReturnCodeWithFiveLowercaseLetters()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new ApplicationDbContext(options);
        var service = new UrlShorteningService(context);
        
        // Act
        var code = await service.GenerateUniqueCode();
        
        // Assert
        Assert.Matches("^[a-z]{5}$", code);
    }
}