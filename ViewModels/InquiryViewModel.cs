using System.ComponentModel.DataAnnotations;
namespace AutoHaven.ViewModels;
public class InquiryViewModel
{
    public int CarId { get; set; }
    public string CarName { get; set; } = "";
    [Required, StringLength(70, MinimumLength = 2)] public string CustomerName { get; set; } = "";
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required, Phone] public string Phone { get; set; } = "";
    [Required] public DateTime PreferredDate { get; set; } = DateTime.Today.AddDays(1);
    [StringLength(500)] public string? Message { get; set; }
}
