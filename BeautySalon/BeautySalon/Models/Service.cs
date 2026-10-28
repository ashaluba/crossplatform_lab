namespace BeautySalonApi.Models;

public class Service
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public decimal BasePrice { get; set; }

    public ServiceCategory? Category { get; set; }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(ServiceName) &&
               CategoryId > 0 &&
               BasePrice >= 0;
    }
}