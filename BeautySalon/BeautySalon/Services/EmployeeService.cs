using System.ComponentModel.DataAnnotations;
using BeautySalonApi.Contracts;
using BeautySalonApi.Data;
using BeautySalonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySalonApi.Services;

public class EmployeeService(AppDbContext context)
{
    public async Task<List<EmployeeResponse>> GetAll()
    {
        return await context.Employees
            .AsNoTracking()
            .OrderBy(e => e.Id)
            .Select(e => new EmployeeResponse(
                e.Id, e.UserId, e.FirstName, e.LastName, e.Phone))
            .ToListAsync();
    }

    public async Task<EmployeeResponse> GetById(int id)
    {
        var employee = await context.Employees
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id)
            ?? throw new KeyNotFoundException("Employee not found.");

        return ToResponse(employee);
    }

    public async Task<EmployeeResponse> Create(
        EmployeeCreateRequest request)
    {
        Validator.ValidateObject(
            request, new ValidationContext(request), true);

        var email = request.Email.Trim();

        if (await context.Users.AnyAsync(u => u.Email == email))
            throw new InvalidOperationException("User already exists.");

        var firstName = request.FirstName.Trim();
        var lastName = request.LastName.Trim();
        var phone = request.Phone.Trim();

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                request.Password),
            Role = "Employee"
        };

        var employee = new Employee
        {
            User = user,
            FirstName = firstName,
            LastName = lastName,
            Phone = phone
        };

        if (!employee.IsValid())
            throw new ValidationException("Invalid employee data.");

        context.Users.Add(user);
        context.Employees.Add(employee);

        await context.SaveChangesAsync();

        return ToResponse(employee);
    }

    public async Task Update(int id, EmployeeRequest request)
    {
        Validator.ValidateObject(
            request, new ValidationContext(request), true);

        var employee = await context.Employees
            .SingleOrDefaultAsync(e => e.Id == id)
            ?? throw new KeyNotFoundException("Employee not found.");

        var user = await context.Users
            .SingleOrDefaultAsync(u => u.Id == request.UserId);

        if (user == null)
            throw new ValidationException(
                "The specified user does not exist.");

        if (user.Role != "Employee")
            throw new ValidationException(
                "The user must have the Employee role.");

        if (await context.Employees.AnyAsync(
            e => e.UserId == request.UserId && e.Id != id))
        {
            throw new InvalidOperationException(
                "This user is already linked to another employee.");
        }

        employee.UserId = request.UserId;
        employee.FirstName = request.FirstName.Trim();
        employee.LastName = request.LastName.Trim();
        employee.Phone = request.Phone.Trim();

        if (!employee.IsValid())
            throw new ValidationException("Invalid employee data.");

        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var employee = await context.Employees
            .SingleOrDefaultAsync(e => e.Id == id)
            ?? throw new KeyNotFoundException("Employee not found.");

        if (await context.Appointments.AnyAsync(
            a => a.EmployeeId == id))
        {
            throw new InvalidOperationException(
                "Cannot delete an employee who has appointments. " +
                "Delete the appointments first.");
        }

        context.Employees.Remove(employee);
        await context.SaveChangesAsync();
    }

    private static EmployeeResponse ToResponse(Employee employee)
    {
        return new EmployeeResponse(
            employee.Id,
            employee.UserId,
            employee.FirstName,
            employee.LastName,
            employee.Phone);
    }
}