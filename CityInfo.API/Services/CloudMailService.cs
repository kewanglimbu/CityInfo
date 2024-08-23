using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private string _mailTo = string.Empty;
        private string _mailFrom = string.Empty;
        private int _port;
        private string _host = string.Empty;
        private string _password = string.Empty;
        
        public CloudMailService(IConfiguration configuration)
        {
            _mailTo = configuration["MailSettings:MailToAddress"];
            _mailFrom = configuration["MailSettings:MailFromAddress"];
            _host = configuration["MailSettings:Host"];
            _port = configuration.GetValue<int>("MailSettings:Port");
            _password = configuration["MailSettings:Password"];
        }

        public async Task Send(string subject, string message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Sender Name", _mailFrom));
            mimeMessage.To.Add(new MailboxAddress("Recipient Name", _mailTo));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                try
                {
                    //client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.CheckCertificateRevocation = false;
                    client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    client.Connect(_host, _port, SecureSocketOptions.StartTls);
                    client.Authenticate(_mailFrom, _password);
                    await client.SendAsync(mimeMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while sending email: {ex.Message}");
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
        }
    }
}
