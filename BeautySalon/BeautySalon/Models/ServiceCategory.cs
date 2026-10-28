namespace BeautySalonApi.Models;

public class ServiceCategory
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(CategoryName);
    }
}