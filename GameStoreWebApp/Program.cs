using GameStoreWebApp.Data;
using GameStoreWebApp.Endpoints;

namespace GameStoreWebApp;

public class Program
{
    public static async Task Main(string[] args)
    { 
        var builder = WebApplication.CreateBuilder(args);

        var connString = builder.Configuration.GetConnectionString("GameStore");

        builder.Services.AddSqlite<GameStoreContext>(connString);
        
        var app = builder.Build();

        app.MapGamesEndpoints();
        app.MapGenresEndpoints();

         await app.MigrateDbAsync();

        app.Run();
    }
}