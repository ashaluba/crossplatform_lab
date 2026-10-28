namespace BeautySalonApi.Models;

public class Qualification
{
    public int Id { get; set; }
    public string QualificationName { get; set; } = string.Empty;

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(QualificationName);
    }
}