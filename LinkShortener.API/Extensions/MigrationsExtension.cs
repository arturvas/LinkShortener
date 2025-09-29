using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.API.Extensions;

public static class MigrationsExtension
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var dbContent = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContent.Database.Migrate();
    }
}