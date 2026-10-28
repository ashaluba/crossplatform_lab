namespace BeautySalonApi.Models;

public class Appointment
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int EmployeeId { get; set; }
    public int ServiceId { get; set; }
    public DateTime DateTime { get; set; }
    public int Duration { get; set; }
    public string Status { get; set; } = "Scheduled";
    public decimal Price { get; set; }

    public Client? Client { get; set; }
    public Employee? Employee { get; set; }
    public Service? Service { get; set; }

    public bool IsValid()
    {
        return ClientId > 0 &&
               EmployeeId > 0 &&
               ServiceId > 0 &&
               Duration > 0 &&
               DateTime > DateTime.UtcNow;
    }
}