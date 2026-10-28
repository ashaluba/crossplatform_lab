namespace BeautySalonApi.Models;

public class Employee
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int QualificationId { get; set; }

    public User? User { get; set; }
    public Qualification? Qualification { get; set; }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}".Trim();
    }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(FirstName) &&
               !string.IsNullOrWhiteSpace(LastName) &&
               !string.IsNullOrWhiteSpace(Phone) &&
               QualificationId > 0;
    }
}