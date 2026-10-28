using BeautySalonApi.Data;
using BeautySalonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySalonApi.Services;

public class ClientService
{
    private readonly AppDbContext _context;

    public ClientService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Client>> GetAll()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client?> GetById(int id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<Client> Create(Client client)
    {
        if (!client.IsValid())
            throw new ArgumentException("Invalid client data.");

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task Update(Client client)
    {
        var current = await _context.Clients.FindAsync(client.Id);
        if (current == null)
            throw new KeyNotFoundException();

        current.FirstName = client.FirstName;
        current.LastName = client.LastName;
        current.Phone = client.Phone;

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
            throw new KeyNotFoundException();

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }
}