
using BeautySalonApi.Contracts;
using BeautySalonApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalonApi.Controllers;

[ApiController]
[Route("api/admins")]
[Authorize(Roles = "Admin")]
public class AdminsController(AuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] AdminCreateRequest request)
    {
        try
        {
            var admin = await authService.CreateAdmin(request);
            return Ok(admin);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
