using Microsoft.AspNetCore.Mvc;
using AutoHaven.Services;
namespace AutoHaven.Controllers;
public class HomeController(AutoHavenStore store) : Controller
{
    public IActionResult Index() => View(store.GetCars().Where(c => c.IsAvailable).Take(6).ToList());
    public IActionResult Privacy() => View();
    public IActionResult Error() => View();
}
