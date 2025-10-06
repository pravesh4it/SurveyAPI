using ABC.Models.DTO;

namespace ABC.Repositories
{

    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendEmailAsync(string toEmails, string subject, string htmlBody, List<EmailAttachment>? attachments = null);
        Task SendEmailWithAttachmentAsync(string toEmail, string subject, string htmlBody, Stream attachmentStream, string attachmentName, string contentType = "application/pdf");
    }

}
