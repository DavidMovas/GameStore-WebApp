using System.ComponentModel.DataAnnotations;

namespace GameStoreWebApp.Dtos;

public record UpdateGameDto
(   
    [Required][StringLength(50)] string Name,
    int GenreId, 
    [Range(1, 1000)] decimal Price,
    DateOnly ReleaseDate
);