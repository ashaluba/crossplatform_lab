using BeautySalonApi.Data;
using BeautySalonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySalonApi.Services;

public class EmployeeService
{
    private readonly AppDbContext _context;

    public EmployeeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<object>> GetAll()
    {
        return await _context.Employees
            .Select(e => new
            {
                e.Id,
                e.FirstName,
                e.LastName,
                e.Phone
            })
            .Cast<object>()
            .ToListAsync();
    }

    public async Task<Employee?> GetById(int id)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Employee> Create(Employee employee)
    {
        if (!employee.IsValid())
            throw new ArgumentException("Invalid employee data.");

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task Update(Employee employee)
    {
        var current = await _context.Employees.FindAsync(employee.Id);
        if (current == null)
            throw new KeyNotFoundException();

        current.FirstName = employee.FirstName;
        current.LastName = employee.LastName;
        current.Phone = employee.Phone;

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
            throw new KeyNotFoundException();

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }
}