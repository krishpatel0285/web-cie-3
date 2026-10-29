namespace AutoHaven.Models;

public class ShowroomInquiry
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string CarName { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public DateTime PreferredDate { get; set; } = DateTime.Today.AddDays(1);
    public string Message { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
