using ABC.Models.Domain;

namespace ABC.Repositories
{
    public interface IPartnerRepository
    {
        Task<List<Client>> GetAllAsync(string ClientTypeId);
        Task<Client> CreateAsync(Client client);
        Task<Client> UpdateAsync(Client client);
        Task<bool> DeleteAsync(Guid clientId);
        Task<Client> GetClentAsync(Guid clientId);
    }
}
