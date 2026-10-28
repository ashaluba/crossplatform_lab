using BeautySalonApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalonApi.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly ReportService _reportService;

    public ReportsController(ReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("appointments-with-details")]
    public async Task<IActionResult> AppointmentsWithDetails()
    {
        var result = await _reportService.GetAppointmentsWithDetails();
        return Ok(result);
    }

    [HttpGet("employees-by-service")]
    public async Task<IActionResult> EmployeesByService()
    {
        var result = await _reportService.GetEmployeesByService();
        return Ok(result);
    }

    [HttpGet("clients-by-appointments")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ClientsByAppointments()
    {
        var result = await _reportService.GetClientsByAppointments();
        return Ok(result);
    }

    [HttpGet("services-summary")]
    public async Task<IActionResult> ServicesSummary()
    {
        var result = await _reportService.GetServicesSummary();
        return Ok(result);
    }
}
