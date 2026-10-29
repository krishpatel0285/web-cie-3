using AutoHaven.Services;
using AutoHaven.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace AutoHaven.Controllers;
public class AccountController(AutoHavenStore store) : Controller
{
    [HttpGet] public IActionResult Register() => View(new RegisterViewModel());
    [HttpPost] [ValidateAntiForgeryToken] public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        if (!store.Register(model.Name, model.Email, model.Password)) { ModelState.AddModelError(nameof(model.Email), "An account with this email already exists."); return View(model); }
        HttpContext.Session.SetString("UserName", model.Name.Trim()); HttpContext.Session.SetString("UserEmail", model.Email.Trim());
        TempData["Success"] = "Welcome to AutoHaven. Your account is ready."; return RedirectToAction("Index", "Home");
    }
    [HttpGet] public IActionResult Login(string? returnUrl = null) { ViewBag.ReturnUrl = returnUrl; return View(new LoginViewModel()); }
    [HttpPost] [ValidateAntiForgeryToken] public IActionResult Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        if (!ModelState.IsValid) return View(model);
        var user = store.Login(model.Email, model.Password);
        if (user == null) { ModelState.AddModelError(string.Empty, "Email or password is incorrect."); return View(model); }
        HttpContext.Session.SetString("UserName", user.Name); HttpContext.Session.SetString("UserEmail", user.Email);
        TempData["Success"] = $"Welcome back, {user.Name}.";
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
        return RedirectToAction("Index", "Home");
    }
    [HttpPost] [ValidateAntiForgeryToken] public IActionResult Logout() { HttpContext.Session.Clear(); return RedirectToAction("Index", "Home"); }
}
