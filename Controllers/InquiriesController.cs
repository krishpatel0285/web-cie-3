using AutoHaven.Models;
using AutoHaven.Services;
using AutoHaven.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace AutoHaven.Controllers;
public class InquiriesController(AutoHavenStore store) : Controller
{
    [HttpGet] public IActionResult Create(int carId)
    {
        var car = store.GetCar(carId); if (car == null) return NotFound();
        return View(new InquiryViewModel { CarId = car.Id, CarName = car.Brand + " " + car.Name, PreferredDate = DateTime.Today.AddDays(1) });
    }
    [HttpPost] [ValidateAntiForgeryToken] public IActionResult Create(InquiryViewModel model)
    {
        var car = store.GetCar(model.CarId);
        if (car == null) return NotFound();
        model.CarName = car.Brand + " " + car.Name;
        if (model.PreferredDate.Date < DateTime.Today) ModelState.AddModelError(nameof(model.PreferredDate), "Please choose today or a future date.");
        if (!ModelState.IsValid) return View(model);
        var saved = store.AddInquiry(new ShowroomInquiry { CarId = car.Id, CarName = model.CarName, CustomerName = model.CustomerName.Trim(), Email = model.Email.Trim(), Phone = model.Phone.Trim(), PreferredDate = model.PreferredDate, Message = model.Message?.Trim() ?? "" });
        return RedirectToAction(nameof(Confirmation), new { id = saved.Id });
    }
    public IActionResult Confirmation(int id) { var inquiry = store.GetInquiries().FirstOrDefault(i => i.Id == id); return inquiry == null ? NotFound() : View(inquiry); }
}
