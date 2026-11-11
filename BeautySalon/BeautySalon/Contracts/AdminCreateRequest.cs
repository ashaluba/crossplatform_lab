using System.ComponentModel.DataAnnotations;

namespace BeautySalonApi.Contracts;

public class AdminCreateRequest
{
    [Required, MaxLength(100)]
    public string FirstName { get; init; } = "";

    [Required, MaxLength(100)]
    public string LastName { get; init; } = "";

    [Required, EmailAddress]
    public string Email { get; init; } = "";

    [Required, MinLength(8)]
    public string Password { get; init; } = "";
}
