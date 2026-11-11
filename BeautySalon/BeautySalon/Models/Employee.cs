namespace BeautySalonApi.Models;

public class Employee
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Phone { get; set; } = "";

    public User? User { get; set; }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(FirstName) &&
               !string.IsNullOrWhiteSpace(LastName) &&
               !string.IsNullOrWhiteSpace(Phone);
    }
}