using System.ComponentModel.DataAnnotations;
using BeautySalonApi.Contracts;
using BeautySalonApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalonApi.Controllers;

[ApiController]
[Route("api/employees")]
[Authorize(Roles = "Admin")]
public class EmployeesController(EmployeeService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAll()
    {
        return Ok(await service.GetAll());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeResponse>> GetById(int id)
    {
        try
        {
            return Ok(await service.GetById(id));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeResponse>> Create(
        EmployeeCreateRequest request)
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
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        EmployeeRequest request)
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
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
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
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}