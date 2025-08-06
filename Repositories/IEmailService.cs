namespace ABC.Repositories
{

    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

}
