using Microsoft.EntityFrameworkCore;

namespace GameStoreWebApp.Data;

public static class DataExtensions
{
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var score = app.Services.CreateScope();

        var dbContext = score.ServiceProvider.GetRequiredService<GameStoreContext>();
        
        await dbContext.Database.MigrateAsync();
    }
}