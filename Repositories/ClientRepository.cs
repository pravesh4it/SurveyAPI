using ABC.Data;
using ABC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ABC.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AbcDbContext dbContext;

        public ClientRepository(AbcDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Client>> GetAllAsync(string clientTypeId)
        {
            return await dbContext.Clients
                          .Where(c => c.ClientTypeId.ToString() == clientTypeId)
                          .ToListAsync();
        }
        public async Task<Client> CreateAsync(Client client)
        {
            try
            {
                await dbContext.Clients.AddAsync(client);
                await dbContext.SaveChangesAsync();
                return client;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in CreateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Client> UpdateAsync(Client client)
        {
            try
            {
                var existingClient = await dbContext.Clients.FindAsync(client.Id);
                if (existingClient == null)
                {
                    throw new Exception("Client not found");
                }

                // Update the properties
                existingClient.Name = client.Name;
                existingClient.Email = client.Email;
                existingClient.Address = client.Address;
                existingClient.C_Variable = client.C_Variable;
                existingClient.ContactPerson = client.ContactPerson;
                existingClient.ContactNo1 = client.ContactNo1;

                dbContext.Clients.Update(existingClient);
                await dbContext.SaveChangesAsync();
                return existingClient;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid clientId)
        {
            try
            {
                var client = await dbContext.Clients.FindAsync(clientId);
                if (client == null)
                {
                    return false; // Client not found
                }

                dbContext.Clients.Remove(client);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                throw;
            }
        }
        public async Task<Client> GetClentAsync(Guid clientId)
        {
            try
            {
                var client = await dbContext.Clients.FindAsync(clientId);
                if (client == null)
                {
                    return null; // Client not found
                }
                return client;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                throw;
            }
        }
    }
}
