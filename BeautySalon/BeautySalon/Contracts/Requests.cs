using System.ComponentModel.DataAnnotations;

namespace BeautySalonApi.Contracts;

public class RegisterRequest
{
    [Required]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    public string LastName { get; init; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required, MinLength(8)]
    public string Password { get; init; } = string.Empty;

    public string Role { get; init; } = "Client";
}

public class LoginRequest
{
    [Required, EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}

public class EmployeeRequest
{
    [Required]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    public string LastName { get; init; } = string.Empty;

    [Required]
    public string Phone { get; init; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int QualificationId { get; init; }
}

public class ClientRequest
{
    [Required]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    public string LastName { get; init; } = string.Empty;

    [Required]
    public string Phone { get; init; } = string.Empty;
}

public class ServiceRequest
{
    [Required]
    public string ServiceName { get; init; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CategoryId { get; init; }

    [Range(typeof(decimal), "0", "99999999.99", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    public decimal BasePrice { get; init; }
}

public class AppointmentRequest
{
    [Range(1, int.MaxValue)]
    public int ClientId { get; init; }

    [Range(1, int.MaxValue)]
    public int EmployeeId { get; init; }

    [Range(1, int.MaxValue)]
    public int ServiceId { get; init; }

    public DateTime DateTime { get; init; }

    [Range(1, int.MaxValue)]
    public int Duration { get; init; }

    [Range(typeof(decimal), "0", "99999999.99", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    public decimal Price { get; init; }

    public string Status { get; init; } = "Scheduled";
}