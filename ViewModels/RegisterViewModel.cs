using System.ComponentModel.DataAnnotations;
namespace AutoHaven.ViewModels;
public class RegisterViewModel
{
    [Required, StringLength(60, MinimumLength = 2)] public string Name { get; set; } = "";
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required, MinLength(8)] public string Password { get; set; } = "";
    [Required, Compare(nameof(Password), ErrorMessage = "Passwords do not match.")] public string ConfirmPassword { get; set; } = "";
}
