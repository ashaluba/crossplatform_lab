using BeautySalonApi.Data;
using BeautySalonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySalonApi.Services;

public class ServiceService
{
    private readonly AppDbContext _context;

    public ServiceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<object>> GetAll()
    {
        return await _context.Services
            .Include(s => s.Category)
            .Select(s => new
            {
                s.Id,
                s.ServiceName,
                s.BasePrice,
                CategoryName = s.Category != null ? s.Category.CategoryName : null
            })
            .Cast<object>()
            .ToListAsync();
    }

    public async Task<Service?> GetById(int id)
    {
        return await _context.Services
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Service> Create(Service service)
    {
        if (!service.IsValid())
            throw new ArgumentException("Invalid service data.");

        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return service;
    }

    public async Task Update(Service service)
    {
        var current = await _context.Services.FindAsync(service.Id);
        if (current == null)
            throw new KeyNotFoundException();

        current.ServiceName = service.ServiceName;
        current.BasePrice = service.BasePrice;
        current.CategoryId = service.CategoryId;

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null)
            throw new KeyNotFoundException();

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
    }
}