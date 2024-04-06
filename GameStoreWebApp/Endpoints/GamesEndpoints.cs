using GameStoreWebApp.Data;
using GameStoreWebApp.Dtos;
using GameStoreWebApp.Entities;
using GameStoreWebApp.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStoreWebApp.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndPointName = "GetGame";
    
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("Games").WithParameterValidation();
        
        //GET /Games
        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.ToGameSummaryDtoDto())
                .AsNoTracking()
                .ToListAsync());
        
        //GET GameById
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game  = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDtoDto());
            
        }).WithName(GetGameEndPointName);
        
        //POST /Games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) => 
            {
                Game game = newGame.ToEntity();

                game.Genre = dbContext.Genres.Find(newGame.GenreId);
                
                
                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                return Results.CreatedAtRoute(GetGameEndPointName, new {id  = game.Id}, game.ToGameDetailsDtoDto());
            }
        );
        
        //PUT /Games
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGameDto, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }
            dbContext.Entry(existingGame)
                .CurrentValues
                .SetValues(updatedGameDto.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.Accepted();
        });
        
        //DELETE /Games/id
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDeleteAsync();
            
            return Results.NoContent();
        });

        return group;
    }
}