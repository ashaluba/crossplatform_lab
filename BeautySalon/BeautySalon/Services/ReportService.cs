using BeautySalonApi.Contracts;
using BeautySalonApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BeautySalonApi.Services;

public class ReportService(AppDbContext context)
{
    public async Task<List<AppointmentsWithDetailsResponse>>
        GetAppointmentsWithDetails()
    {
        return await context.Appointments
            .AsNoTracking()
            .Select(a => new AppointmentsWithDetailsResponse(
                a.Id,
                a.DateTime,
                a.Status,
                a.Client!.FirstName + " " + a.Client.LastName,
                a.Employee!.FirstName + " " + a.Employee.LastName,
                a.Service!.ServiceName,
                a.Price))
            .ToListAsync();
    }

    public async Task<List<EmployeesByServiceResponse>> GetEmployeesByService()
    {
        var appointments = await context.Appointments
            .AsNoTracking()
            .Include(a => a.Employee)
            .Include(a => a.Service)
            .ToListAsync();

        return appointments
            .GroupBy(a => new
            {
                Employee = a.Employee != null
                    ? a.Employee.FirstName + " " + a.Employee.LastName
                    : "Unknown",
                Service = a.Service != null
                    ? a.Service.ServiceName
                    : "Unknown"
            })
            .Select(g => new EmployeesByServiceResponse(
                g.Key.Employee,
                g.Key.Service,
                g.Count()))
            .OrderByDescending(x => x.AppointmentsCount)
            .ToList();
    }

    public async Task<List<ClientsByAppointmentsResponse>>
    GetClientsByAppointments()
    {
        var appointments = await context.Appointments
            .AsNoTracking()
            .Include(a => a.Client)
            .ToListAsync();

        return appointments
            .GroupBy(a => new
            {
                Client = a.Client != null
                    ? a.Client.FirstName + " " + a.Client.LastName
                    : "Unknown"
            })
            .Select(g => new ClientsByAppointmentsResponse(
                g.Key.Client,
                g.Count()))
            .OrderByDescending(x => x.AppointmentsCount)
            .ToList();
    }

    public async Task<List<ServicesSummaryResponse>> GetServicesSummary()
    {
        return await context.Services
            .AsNoTracking()
            .Select(s => new ServicesSummaryResponse(
                s.Id,
                s.ServiceName,
                s.BasePrice,
                context.Appointments.Count(a => a.ServiceId == s.Id)))
            .ToListAsync();
    }
}