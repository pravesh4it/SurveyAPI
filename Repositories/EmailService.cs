using ABC.Models.Domain;
using ABC.Models.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Runtime;
using System.Threading.Tasks;

namespace ABC.Repositories
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string toEmails, string subject, string htmlBody, List<EmailAttachment>? attachments = null)
        {
            var message = new MimeMessage();

            // From
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));

            // To: support comma separated list
            if (!string.IsNullOrWhiteSpace(toEmails))
            {
                var parts = toEmails.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    message.To.Add(MailboxAddress.Parse(p.Trim()));
                }
            }

            message.Subject = subject ?? string.Empty;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlBody ?? string.Empty
            };

            // Attach files if provided
            if (attachments != null)
            {
                foreach (var att in attachments)
                {
                    if (att?.Content == null || att.Content.Length == 0) continue;

                    // Use MemoryStream to create attachment
                    using var ms = new MemoryStream(att.Content, writable: false);
                    // MimeKit will copy the stream, so it's safe to dispose afterwards.
                    var contentType = string.IsNullOrWhiteSpace(att.ContentType) ? "application/octet-stream" : att.ContentType;
                    builder.Attachments.Add(att.FileName ?? "attachment", ms, ContentType.Parse(contentType));
                }
            }

            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            // Choose secure option: StartTls is common (port 587). If you use port 465 use SslOnConnect.
            SecureSocketOptions socketOptions = SecureSocketOptions.StartTls;
            if (_emailSettings.Port == 465)
            {
                socketOptions = SecureSocketOptions.SslOnConnect;
            }

            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, socketOptions);

            // Authenticate only if username/password provided
            if (!string.IsNullOrWhiteSpace(_emailSettings.SenderEmail) && !string.IsNullOrWhiteSpace(_emailSettings.Password))
            {
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                var builder = new BodyBuilder { HtmlBody = body };
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        // Convenience wrapper to accept a Stream (your original signature)
        public async Task SendEmailWithAttachmentAsync(
            string toEmail,
            string subject,
            string htmlBody,
            Stream attachmentStream,
            string attachmentName,
            string contentType = "application/pdf")
        {
            if (attachmentStream == null)
                throw new ArgumentNullException(nameof(attachmentStream));

            // Ensure stream position at beginning
            if (attachmentStream.CanSeek)
            {
                try { attachmentStream.Position = 0; } catch { /* ignore */ }
            }

            // Read stream into byte[] (use buffering)
            byte[] data;
            using (var ms = new MemoryStream())
            {
                await attachmentStream.CopyToAsync(ms);
                data = ms.ToArray();
            }

            var att = new EmailAttachment
            {
                FileName = attachmentName ?? "attachment.pdf",
                ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/pdf" : contentType,
                Content = data
            };

            await SendEmailAsync(toEmail, subject, htmlBody, new List<EmailAttachment> { att });
        }

    }
}
