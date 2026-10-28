using System.ComponentModel.DataAnnotations;
using BeautySalonApi.Contracts;
using BeautySalonApi.Data;
using BeautySalonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySalonApi.Services;

public class ServiceService(AppDbContext context)
{
    public async Task<List<ServiceResponse>> GetAll()
    {
        return await context.Services
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .Select(s => new ServiceResponse(
                s.Id, s.ServiceName, s.BasePrice))
            .ToListAsync();
    }

    public async Task<ServiceResponse> GetById(int id)
    {
        var service = await context.Services
            .AsNoTracking()
            .SingleOrDefaultAsync(s => s.Id == id)
            ?? throw new KeyNotFoundException("Service not found.");

        return ToResponse(service);
    }

    public async Task<ServiceResponse> Create(ServiceRequest request)
    {
        Validator.ValidateObject(
            request, new ValidationContext(request), true);

        var service = new Service
        {
            ServiceName = request.ServiceName.Trim(),
            BasePrice = request.BasePrice
        };

        if (!service.IsValid())
            throw new ValidationException("Invalid service data.");

        context.Services.Add(service);
        await context.SaveChangesAsync();

        return ToResponse(service);
    }

    public async Task Update(int id, ServiceRequest request)
    {
        Validator.ValidateObject(
            request, new ValidationContext(request), true);

        var service = await context.Services
            .SingleOrDefaultAsync(s => s.Id == id)
            ?? throw new KeyNotFoundException("Service not found.");

        service.ServiceName = request.ServiceName.Trim();
        service.BasePrice = request.BasePrice;

        if (!service.IsValid())
            throw new ValidationException("Invalid service data.");

        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var service = await context.Services
            .SingleOrDefaultAsync(s => s.Id == id)
            ?? throw new KeyNotFoundException("Service not found.");

        if (await context.Appointments.AnyAsync(a => a.ServiceId == id))
        {
            throw new InvalidOperationException(
                "Cannot delete a service used by appointments. " +
                "Delete the appointments first.");
        }

        context.Services.Remove(service);
        await context.SaveChangesAsync();
    }

    private static ServiceResponse ToResponse(Service service)
    {
        return new ServiceResponse(
            service.Id,
            service.ServiceName,
            service.BasePrice);
    }
}