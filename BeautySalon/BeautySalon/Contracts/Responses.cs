namespace BeautySalonApi.Contracts;

public record UserResponse(
    int Id,
    string Email,
    string FirstName,
    string LastName,
    string Role);

public record AuthResponse(
    string Token,
    DateTimeOffset ExpiresAt,
    UserResponse User);

public record ClientResponse(
    int Id,
    string FirstName,
    string LastName,
    string Phone);

public record EmployeeResponse(
    int Id,
    int UserId,
    string FirstName,
    string LastName,
    string Phone);

public record ServiceResponse(
    int Id,
    string ServiceName,
    decimal BasePrice);

public record AppointmentResponse(
    int Id,
    int ClientId,
    int EmployeeId,
    int ServiceId,
    DateTime DateTime,
    int Duration,
    string Status,
    decimal Price,
    string ClientName,
    string EmployeeName,
    string ServiceName);

public record AppointmentsWithDetailsResponse(
    int Id,
    DateTime DateTime,
    string Status,
    string Client,
    string Employee,
    string Service,
    decimal Price);

public record EmployeesByServiceResponse(
    string Employee,
    string Service,
    int AppointmentsCount);

public record ClientsByAppointmentsResponse(
    string Client,
    int AppointmentsCount);

public record ServicesSummaryResponse(
    int Id,
    string ServiceName,
    decimal BasePrice,
    int AppointmentsCount);