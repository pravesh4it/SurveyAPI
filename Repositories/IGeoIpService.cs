namespace ABC.Repositories
{
    public interface IGeoIpService
    {
        Task<string> GetCountryCodeAsync(string ip);
    }

}
