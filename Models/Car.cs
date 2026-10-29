namespace AutoHaven.Models;

public class Car
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Brand { get; set; } = "";
    public string Category { get; set; } = "Sedan";
    public int Year { get; set; }
    public decimal Price { get; set; }
    public string Fuel { get; set; } = "Petrol";
    public string Transmission { get; set; } = "Automatic";
    public int Seats { get; set; } = 5;
    public int Mileage { get; set; }
    public string ImageUrl { get; set; } = "";
    public string Badge { get; set; } = "Featured";
    public string Description { get; set; } = "";
    public bool IsAvailable { get; set; } = true;
}
