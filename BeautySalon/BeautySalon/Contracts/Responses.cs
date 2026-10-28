namespace BeautySalonApi.Contracts;

public record UserResponse(int Id, string Email, string FirstName, string LastName, string Role);

public record AuthResponse(string Token, DateTimeOffset ExpiresAt, UserResponse User);

public record EmployeeResponse(
    int Id,
    string FirstName,
    string LastName,
    string Phone,
    int QualificationId,
    string? QualificationName);

public record ClientResponse(
    int Id,
    string FirstName,
    string LastName,
    string Phone);

public record ServiceResponse(
    int Id,
    string ServiceName,
    decimal BasePrice,
    int CategoryId,
    string? CategoryName);

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
    string ServiceName,
    string CategoryName);

public record QualificationResponse(int Id, string QualificationName);

public record ServiceCategoryResponse(int Id, string CategoryName);

public record ReportResponse(
    int TotalEmployees,
    int TotalClients,
    int TotalServices,
    int TotalAppointments);

public record AppointmentsWithDetailsResponse(
    int Id,
    DateTime DateTime,
    string Status,
    string Client,
    string Employee,
    string Qualification,
    string Service,
    string Category,
    decimal Price);

public record EmployeesByServiceResponse(
    string Employee,
    string Service,
    string Category);

public record ClientsByAppointmentsResponse(
    string Client,
    int AppointmentsCount);

public record ServicesSummaryResponse(
    int Id,
    string ServiceName,
    decimal BasePrice,
    string Category);