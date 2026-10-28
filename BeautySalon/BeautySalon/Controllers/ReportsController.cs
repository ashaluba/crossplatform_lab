using BeautySalonApi.Contracts;
using BeautySalonApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalonApi.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController(ReportService service) : ControllerBase
{
    [HttpGet("appointments-with-details")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<AppointmentsWithDetailsResponse>>>
        AppointmentsWithDetails()
    {
        return Ok(await service.GetAppointmentsWithDetails());
    }

    [HttpGet("employees-by-service")]
    public async Task<ActionResult<List<EmployeesByServiceResponse>>>
        EmployeesByService()
    {
        return Ok(await service.GetEmployeesByService());
    }

    [HttpGet("clients-by-appointments")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<ClientsByAppointmentsResponse>>>
        ClientsByAppointments()
    {
        return Ok(await service.GetClientsByAppointments());
    }

    [HttpGet("services-summary")]
    public async Task<ActionResult<List<ServicesSummaryResponse>>>
        ServicesSummary()
    {
        return Ok(await service.GetServicesSummary());
    }
}