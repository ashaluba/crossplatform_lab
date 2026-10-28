using System.ComponentModel.DataAnnotations;
using BeautySalonApi.Contracts;
using BeautySalonApi.Data;
using BeautySalonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySalonApi.Services;

public class ClientService(AppDbContext context)
{
    public async Task<List<ClientResponse>> GetAll()
    {
        return await context.Clients
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Select(c => new ClientResponse(
                c.Id, c.FirstName, c.LastName, c.Phone))
            .ToListAsync();
    }

    public async Task<ClientResponse> GetById(int id)
    {
        var client = await context.Clients
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Id == id)
            ?? throw new KeyNotFoundException("Client not found.");

        return ToResponse(client);
    }

    public async Task<ClientResponse> Create(ClientRequest request)
    {
        Validator.ValidateObject(
            request, new ValidationContext(request), true);

        var client = new Client
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Phone = request.Phone.Trim()
        };

        if (!client.IsValid())
            throw new ValidationException("Invalid client data.");

        context.Clients.Add(client);
        await context.SaveChangesAsync();

        return ToResponse(client);
    }

    public async Task Update(int id, ClientRequest request)
    {
        Validator.ValidateObject(
            request, new ValidationContext(request), true);

        var client = await context.Clients
            .SingleOrDefaultAsync(c => c.Id == id)
            ?? throw new KeyNotFoundException("Client not found.");

        client.FirstName = request.FirstName.Trim();
        client.LastName = request.LastName.Trim();
        client.Phone = request.Phone.Trim();

        if (!client.IsValid())
            throw new ValidationException("Invalid client data.");

        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var client = await context.Clients
            .SingleOrDefaultAsync(c => c.Id == id)
            ?? throw new KeyNotFoundException("Client not found.");

        if (await context.Appointments.AnyAsync(a => a.ClientId == id))
        {
            throw new InvalidOperationException(
                "Cannot delete a client who has appointments. " +
                "Delete the appointments first.");
        }

        context.Clients.Remove(client);
        await context.SaveChangesAsync();
    }

    private static ClientResponse ToResponse(Client client)
    {
        return new ClientResponse(
            client.Id,
            client.FirstName,
            client.LastName,
            client.Phone);
    }
}