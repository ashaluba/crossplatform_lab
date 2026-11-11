using System.ComponentModel.DataAnnotations;

namespace BeautySalonApi.Contracts;

public class RegisterRequest
{
    [Required, MaxLength(100)]
    public string FirstName { get; init; } = "";

    [Required, MaxLength(100)]
    public string LastName { get; init; } = "";

    [Required, EmailAddress]
    public string Email { get; init; } = "";

    [Required, MinLength(8)]
    public string Password { get; init; } = "";

    [Required, MaxLength(20)]
    public string Phone { get; init; } = "";
}

public class LoginRequest
{
    [Required, EmailAddress]
    public string Email { get; init; } = "";

    [Required]
    public string Password { get; init; } = "";
}

public class EmployeeCreateRequest
{
    [Required, MaxLength(100)]
    public string FirstName { get; init; } = "";

    [Required, MaxLength(100)]
    public string LastName { get; init; } = "";

    [Required, EmailAddress]
    public string Email { get; init; } = "";

    [Required, MinLength(8)]
    public string Password { get; init; } = "";

    [Required, MaxLength(20)]
    public string Phone { get; init; } = "";
}

public class EmployeeRequest
{
    [Range(1, int.MaxValue)]
    public int UserId { get; init; }

    [Required, MaxLength(100)]
    public string FirstName { get; init; } = "";

    [Required, MaxLength(100)]
    public string LastName { get; init; } = "";

    [Required, MaxLength(20)]
    public string Phone { get; init; } = "";
}

public class ClientRequest
{
    [Required, MaxLength(100)]
    public string FirstName { get; init; } = "";

    [Required, MaxLength(100)]
    public string LastName { get; init; } = "";

    [Required, MaxLength(20)]
    public string Phone { get; init; } = "";
}

public class ServiceRequest
{
    [Required, MaxLength(150)]
    public string ServiceName { get; init; } = "";

    [Range(typeof(decimal), "0", "99999999.99",
        ParseLimitsInInvariantCulture = true,
        ConvertValueInInvariantCulture = true)]
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

    [Range(typeof(decimal), "0", "99999999.99",
        ParseLimitsInInvariantCulture = true,
        ConvertValueInInvariantCulture = true)]
    public decimal Price { get; init; }

    [Required, MaxLength(30)]
    public string Status { get; init; } = "Scheduled";
}