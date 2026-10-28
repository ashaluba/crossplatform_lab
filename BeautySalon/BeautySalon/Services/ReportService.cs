using BeautySalonApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BeautySalonApi.Services;

public class ReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<object> GetAppointmentsWithDetails()
    {
        return await _context.Appointments
            .Include(a => a.Client)
            .Include(a => a.Employee)
            .ThenInclude(e => e.Qualification)
            .Include(a => a.Service)
            .ThenInclude(s => s.Category)
            .Select(a => new
            {
                a.Id,
                a.DateTime,
                a.Status,
                Client = a.Client.FirstName + " " + a.Client.LastName,
                Employee = a.Employee.FirstName + " " + a.Employee.LastName,
                Qualification = a.Employee.Qualification.QualificationName,
                Service = a.Service.ServiceName,
                Category = a.Service.Category.CategoryName,
                a.Price
            })
            .ToListAsync();
    }

    public async Task<object> GetEmployeesByService()
    {
        return await _context.EmployeeServices
            .Include(es => es.Employee)
            .Include(es => es.Service)
            .ThenInclude(s => s.Category)
            .Select(es => new
            {
                Employee = es.Employee.FirstName + " " + es.Employee.LastName,
                Service = es.Service.ServiceName,
                Category = es.Service.Category.CategoryName
            })
            .ToListAsync();
    }

    public async Task<object> GetClientsByAppointments()
    {
        return await _context.Appointments
            .Include(a => a.Client)
            .GroupBy(a => new { a.ClientId, a.Client.FirstName, a.Client.LastName })
            .Select(g => new
            {
                Client = g.Key.FirstName + " " + g.Key.LastName,
                AppointmentsCount = g.Count()
            })
            .OrderByDescending(x => x.AppointmentsCount)
            .ToListAsync();
    }

    public async Task<object> GetServicesSummary()
    {
        return await _context.Services
            .Include(s => s.Category)
            .Select(s => new
            {
                s.Id,
                s.ServiceName,
                s.BasePrice,
                Category = s.Category.CategoryName
            })
            .ToListAsync();
    }
}