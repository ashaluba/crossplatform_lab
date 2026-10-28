namespace BeautySalonApi.Models;

public class Client
{
    public int Id { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public string GetFullName()
    {
        return $"{FirstName} {LastName}".Trim();
    }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(FirstName) &&
               !string.IsNullOrWhiteSpace(LastName) &&
               !string.IsNullOrWhiteSpace(Phone);
    }
}