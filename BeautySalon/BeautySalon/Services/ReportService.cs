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

    public async Task<List<EmployeesByServiceResponse>>
        GetEmployeesByService()
    {
        return await context.Appointments
            .AsNoTracking()
            .GroupBy(a => new
            {
                a.EmployeeId,
                Employee = a.Employee!.FirstName + " " + a.Employee.LastName,
                a.ServiceId,
                Service = a.Service!.ServiceName
            })
            .Select(g => new EmployeesByServiceResponse(
                g.Key.Employee,
                g.Key.Service,
                g.Count()))
            .OrderByDescending(x => x.AppointmentsCount)
            .ToListAsync();
    }

    public async Task<List<ClientsByAppointmentsResponse>>
        GetClientsByAppointments()
    {
        return await context.Appointments
            .AsNoTracking()
            .GroupBy(a => new
            {
                a.ClientId,
                a.Client!.FirstName,
                a.Client.LastName
            })
            .Select(g => new ClientsByAppointmentsResponse(
                g.Key.FirstName + " " + g.Key.LastName,
                g.Count()))
            .OrderByDescending(x => x.AppointmentsCount)
            .ToListAsync();
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