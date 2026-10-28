namespace BeautySalonApi.Models;

public class EmployeeServiceAssignment
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ServiceId { get; set; }

    public Employee? Employee { get; set; }
    public Service? Service { get; set; }
}