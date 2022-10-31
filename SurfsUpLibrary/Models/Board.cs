using System.ComponentModel.DataAnnotations;

namespace SurfsUpLibrary.Models;

public enum BoardState
{
    Available,
    Rented
}

public class Board
{
    public Guid BoardId { get; set; }

    [StringLength(255, MinimumLength = 1)]
    [Required]
    public string? BoardName { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    
    [StringLength(255, MinimumLength = 1)]
    [Required]
    public string? Type { get; set; }
    
    public double Length { get; set; }
    
    public double Width { get; set; }
    
    public double Thickness { get; set; }
    
    public double Volume { get; set; }
    
    public BoardState State { get; set; } = BoardState.Available;

    private string? picture;
    public string? Picture
    {
        get { return picture; }
        set { picture = value ?? ""; }
    }

    private string? equipment;
    public string? Equipment
    {
        get { return equipment; }
        set { equipment = value ?? ""; }
    }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}
