using GameStoreWebApp.Data;
using GameStoreWebApp.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStoreWebApp.Endpoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("Generes");

        group.MapGet("/", async (GameStoreContext dbContext) => 
            
            await dbContext.Genres
                .Select(genre => genre.ToGenreDto())
                .AsNoTracking()
                .ToListAsync());

        return group;
    }
}