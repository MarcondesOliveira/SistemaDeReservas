namespace SistemaDeReservas.Domain.Repositories
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
