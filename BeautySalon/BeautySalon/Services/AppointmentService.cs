using BeautySalonApi.Data;
using BeautySalonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySalonApi.Services;

public class AppointmentService
{
    private readonly AppDbContext _context;

    public AppointmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<object>> GetAll()
    {
        return await _context.Appointments
            .Include(a => a.Client)
            .Include(a => a.Employee)
            .Include(a => a.Service)
            .Select(a => new
            {
                a.Id,
                a.DateTime,
                a.Duration,
                a.Status,
                a.Price,
                Client = a.Client.FirstName + " " + a.Client.LastName,
                Employee = a.Employee.FirstName + " " + a.Employee.LastName,
                Service = a.Service.ServiceName
            })
            .Cast<object>()
            .ToListAsync();
    }

    public async Task<Appointment?> GetById(int id)
    {
        return await _context.Appointments
            .Include(a => a.Client)
            .Include(a => a.Employee)
            .Include(a => a.Service)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Appointment> Create(Appointment appointment)
    {
        if (!appointment.IsValid())
            throw new ArgumentException("Invalid appointment data.");

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task Update(Appointment appointment)
    {
        var current = await _context.Appointments.FindAsync(appointment.Id);
        if (current == null)
            throw new KeyNotFoundException();

        current.ClientId = appointment.ClientId;
        current.EmployeeId = appointment.EmployeeId;
        current.ServiceId = appointment.ServiceId;
        current.DateTime = appointment.DateTime;
        current.Duration = appointment.Duration;
        current.Status = appointment.Status;
        current.Price = appointment.Price;

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
            throw new KeyNotFoundException();

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
    }
}