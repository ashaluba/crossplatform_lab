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
            .Include(a => a.Service)
            .Select(a => new
            {
                a.Id,
                a.DateTime,
                a.Status,
                Client = a.Client.FirstName + " " + a.Client.LastName,
                Employee = a.Employee.FirstName + " " + a.Employee.LastName,
                Service = a.Service.ServiceName,
                a.Price
            })
            .ToListAsync();
    }

    public async Task<object> GetEmployeesByService()
    {
        return await _context.Appointments
            .Include(a => a.Employee)
            .Include(a => a.Service)
            .GroupBy(a => new
            {
                EmployeeId = a.EmployeeId,
                Employee = a.Employee.FirstName + " " + a.Employee.LastName,
                ServiceId = a.ServiceId,
                Service = a.Service.ServiceName
            })
            .Select(g => new
            {
                Employee = g.Key.Employee,
                Service = g.Key.Service,
                AppointmentsCount = g.Count()
            })
            .OrderByDescending(x => x.AppointmentsCount)
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
            .Select(s => new
            {
                s.Id,
                s.ServiceName,
                s.BasePrice,
                AppointmentsCount = _context.Appointments.Count(a => a.ServiceId == s.Id)
            })
            .ToListAsync();
    }
}