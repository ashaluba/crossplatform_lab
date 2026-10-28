using BeautySalonApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeautySalonApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommonController : ControllerBase
{
    private readonly AppDbContext _context;

    public CommonController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("report")]
    public async Task<IActionResult> GetReport()
    {
        var employees = await _context.Employees.CountAsync();
        var clients = await _context.Clients.CountAsync();
        var services = await _context.Services.CountAsync();
        var appointments = await _context.Appointments.CountAsync();

        return Ok(new
        {
            employees,
            clients,
            services,
            appointments
        });
    }
}