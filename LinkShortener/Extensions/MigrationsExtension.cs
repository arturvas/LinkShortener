using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Extensions;

public static class MigrationsExtension
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var dbContent = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            dbContent.Database.Migrate();
        }
        catch (SqlException e) when (e.Number == 40615)
        {
            app.Logger.LogError("Azure SQL blocked the actual IP. See firewall rules '{Server}'. Details: {Message}", "sql-linkshortener-004", e.Message);
            throw;
        }
    }
}