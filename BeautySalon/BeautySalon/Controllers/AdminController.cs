using BeautySalonApi.Models;
using BeautySalonApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalonApi.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly EmployeeService _employeeService;
    private readonly ClientService _clientService;
    private readonly ServiceService _serviceService;

    public AdminController(EmployeeService employeeService, ClientService clientService, ServiceService serviceService)
    {
        _employeeService = employeeService;
        _clientService = clientService;
        _serviceService = serviceService;
    }

    [HttpGet("employees")]
    public async Task<IActionResult> GetEmployees()
    {
        var result = await _employeeService.GetAll();
        return Ok(result);
    }

    [HttpGet("clients")]
    public async Task<IActionResult> GetClients()
    {
        var result = await _clientService.GetAll();
        return Ok(result);
    }

    [HttpGet("services")]
    public async Task<IActionResult> GetServices()
    {
        var result = await _serviceService.GetAll();
        return Ok(result);
    }

    [HttpPost("employees")]
    public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
    {
        try
        {
            var result = await _employeeService.Create(employee);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("clients")]
    public async Task<IActionResult> CreateClient([FromBody] Client client)
    {
        try
        {
            var result = await _clientService.Create(client);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("services")]
    public async Task<IActionResult> CreateService([FromBody] Service service)
    {
        try
        {
            var result = await _serviceService.Create(service);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}