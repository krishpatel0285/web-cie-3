using AutoHaven.Services;
using Microsoft.AspNetCore.Mvc;
namespace AutoHaven.Controllers;
public class CarsController(AutoHavenStore store) : Controller
{
    public IActionResult Index(string? q, string? category, string? fuel, string? sort)
    {
        var cars = store.GetCars().Where(c => c.IsAvailable);
        if (!string.IsNullOrWhiteSpace(q)) cars = cars.Where(c => (c.Name + " " + c.Brand + " " + c.Category).Contains(q.Trim(), StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(category) && category != "All") cars = cars.Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(fuel) && fuel != "All") cars = cars.Where(c => c.Fuel.Equals(fuel, StringComparison.OrdinalIgnoreCase));
        cars = sort switch { "price-low" => cars.OrderBy(c => c.Price), "price-high" => cars.OrderByDescending(c => c.Price), "year" => cars.OrderByDescending(c => c.Year), _ => cars.OrderBy(c => c.Brand) };
        ViewBag.Query = q; ViewBag.Category = category; ViewBag.Fuel = fuel; ViewBag.Sort = sort;
        return View(cars.ToList());
    }
    public IActionResult Details(int id) { var car = store.GetCar(id); return car == null ? NotFound() : View(car); }
}
