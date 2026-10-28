using System.ComponentModel.DataAnnotations;
using BeautySalonApi.Contracts;
using BeautySalonApi.Data;
using BeautySalonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySalonApi.Services;

public class AppointmentService(AppDbContext context)
{
    private IQueryable<Appointment> QueryWithDetails()
    {
        return context.Appointments
            .AsNoTracking()
            .Include(a => a.Client)
            .Include(a => a.Employee)
            .Include(a => a.Service);
    }

    public async Task<List<AppointmentResponse>> GetAll()
    {
        return await QueryWithDetails()
            .OrderBy(a => a.DateTime)
            .Select(a => new AppointmentResponse(
                a.Id,
                a.ClientId,
                a.EmployeeId,
                a.ServiceId,
                a.DateTime,
                a.Duration,
                a.Status,
                a.Price,
                a.Client!.FirstName + " " + a.Client.LastName,
                a.Employee!.FirstName + " " + a.Employee.LastName,
                a.Service!.ServiceName))
            .ToListAsync();
    }

    public async Task<AppointmentResponse> GetById(int id)
    {
        var appointment = await QueryWithDetails()
            .Where(a => a.Id == id)
            .Select(a => new AppointmentResponse(
                a.Id,
                a.ClientId,
                a.EmployeeId,
                a.ServiceId,
                a.DateTime,
                a.Duration,
                a.Status,
                a.Price,
                a.Client!.FirstName + " " + a.Client.LastName,
                a.Employee!.FirstName + " " + a.Employee.LastName,
                a.Service!.ServiceName))
            .SingleOrDefaultAsync();

        return appointment
            ?? throw new KeyNotFoundException("Appointment not found.");
    }

    public async Task<AppointmentResponse> Create(AppointmentRequest request)
    {
        Validate(request);
        await EnsureReferencesExist(request);

        var appointment = new Appointment
        {
            ClientId = request.ClientId,
            EmployeeId = request.EmployeeId,
            ServiceId = request.ServiceId,
            DateTime = request.DateTime,
            Duration = request.Duration,
            Status = request.Status.Trim(),
            Price = request.Price
        };

        if (!appointment.IsValid())
        {
            throw new ValidationException(
                "Invalid appointment data. Date must be in the future " +
                "and duration must be positive.");
        }

        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        return await GetById(appointment.Id);
    }

    public async Task Update(int id, AppointmentRequest request)
    {
        Validate(request);
        await EnsureReferencesExist(request);

        var appointment = await context.Appointments
            .SingleOrDefaultAsync(a => a.Id == id)
            ?? throw new KeyNotFoundException("Appointment not found.");

        appointment.ClientId = request.ClientId;
        appointment.EmployeeId = request.EmployeeId;
        appointment.ServiceId = request.ServiceId;
        appointment.DateTime = request.DateTime;
        appointment.Duration = request.Duration;
        appointment.Status = request.Status.Trim();
        appointment.Price = request.Price;

        if (!appointment.IsValid())
        {
            throw new ValidationException(
                "Invalid appointment data. Date must be in the future " +
                "and duration must be positive.");
        }

        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var appointment = await context.Appointments
            .SingleOrDefaultAsync(a => a.Id == id)
            ?? throw new KeyNotFoundException("Appointment not found.");

        context.Appointments.Remove(appointment);
        await context.SaveChangesAsync();
    }

    private static void Validate(AppointmentRequest request)
    {
        Validator.ValidateObject(
            request, new ValidationContext(request), true);
    }

    private async Task EnsureReferencesExist(AppointmentRequest request)
    {
        if (!await context.Clients.AnyAsync(c => c.Id == request.ClientId))
            throw new ValidationException("The specified client does not exist.");

        if (!await context.Employees.AnyAsync(e => e.Id == request.EmployeeId))
            throw new ValidationException("The specified employee does not exist.");

        if (!await context.Services.AnyAsync(s => s.Id == request.ServiceId))
            throw new ValidationException("The specified service does not exist.");
    }
}