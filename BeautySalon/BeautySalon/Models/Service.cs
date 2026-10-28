namespace BeautySalonApi.Models;

public class Service
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(ServiceName) &&
               BasePrice >= 0;
    }
}