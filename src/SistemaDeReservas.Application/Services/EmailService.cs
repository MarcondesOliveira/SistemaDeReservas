using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using SistemaDeReservas.Domain.Repositories;
using System.Threading.Tasks;

namespace SistemaDeReservas.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            _logger.LogInformation("Preparando para enviar email...");

            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_configuration["EmailFromName"], _configuration["EmailFromAddress"]));
                emailMessage.To.Add(new MailboxAddress("", toEmail));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("plain") { Text = message };

                using (var client = new SmtpClient())
                {
                    client.CheckCertificateRevocation = false;

                    _logger.LogInformation("Conectando ao servidor SMTP...");

                    await client.ConnectAsync(_configuration["SmtpServer"], int.Parse(_configuration["SmtpPort"]
                        ?? throw new Exception("Porta inacessível")), SecureSocketOptions.StartTls);

                    _logger.LogInformation("Autenticando no servidor SMTP...");

                    await client.AuthenticateAsync(_configuration["SmtpUsername"], _configuration["SmtpPassword"]);
                    _logger.LogInformation("Enviando email...");

                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }

                _logger.LogInformation($"Email enviado para {toEmail} com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar email.");
                throw;
            }
        }
    }

}
