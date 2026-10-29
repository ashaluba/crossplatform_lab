using System.ComponentModel.DataAnnotations;
using BeautySalonApi.Contracts;
using BeautySalonApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeautySalonApi.Controllers;

[ApiController]
[Route("api/appointments")]
[Authorize]
public class AppointmentsController(AppointmentService service)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AppointmentResponse>>> GetAll()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var userId))
            return Unauthorized();

        var isAdmin = User.IsInRole("Admin");
        var isEmployee = User.IsInRole("Employee");

        return Ok(await service.GetAll(userId, isAdmin, isEmployee));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppointmentResponse>> GetById(int id)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var userId))
            return Unauthorized();

        var isAdmin = User.IsInRole("Admin");
        var isEmployee = User.IsInRole("Employee");

        try
        {
            return Ok(await service.GetById(
                id, userId, isAdmin, isEmployee));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AppointmentResponse>> Create(
        AppointmentRequest request)
    {
        try
        {
            var result = await service.Create(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int id,
        AppointmentRequest request)
    {
        try
        {
            await service.Update(id, request);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await service.Delete(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}