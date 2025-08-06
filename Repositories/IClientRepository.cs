using ABC.Models.Domain;

namespace ABC.Repositories
{
    public interface IClientRepository
    {
        Task<List<Client>> GetAllAsync(string clientTypeId);
        Task<Client> CreateAsync(Client client);
        Task<Client> UpdateAsync(Client client);
        Task<bool> DeleteAsync(Guid clientId);
        Task<Client> GetClentAsync(Guid clientId);
    }
}
