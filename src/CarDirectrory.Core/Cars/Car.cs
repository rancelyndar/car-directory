namespace CarDirectrory.Core;

public class Car
{
    public string StateNumber { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public int ReleaseYear { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}