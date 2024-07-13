using Microsoft.Extensions.Logging;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Enum;
using SistemaDeReservas.Domain.Events;
using SistemaDeReservas.Domain.Repositories;

namespace SistemaDeReservas.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IUsuarioRepository usuarioRepository, ILogger<NotificationService> logger, IEmailService emailService)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task EnviarNotificacaoAsync(string mensagem, int usuarioId, Tipo tipo)
        {
            _logger.LogInformation("Iniciando o envio da notificação para o usuário {UsuarioId} com a mensagem: {Mensagem}", usuarioId, mensagem);

            var usuario = _usuarioRepository.GetById(usuarioId);

            if (usuario != null)
            {
                var notificacao = new Notificacao
                {
                    Mensagem = mensagem,
                    Tipo = tipo,
                    Destinatario = usuario.Email // Ajuste conforme necessário
                };

                // Aqui você pode implementar a lógica para enviar a notificação
                switch (tipo)
                {
                    case Tipo.Email:
                        await _emailService.SendEmailAsync(notificacao.Destinatario, "Notificação de Reserva", notificacao.Mensagem);
                        break;
                    case Tipo.SMS:
                        EnviarSMS(notificacao);
                        break;
                    case Tipo.Push:
                        EnviarPush(notificacao);
                        break;
                }
            }
            else
            {
                _logger.LogWarning("Usuário não encontrado: {UsuarioId}", usuarioId);
            }
        }

        public async Task HandleAsync(ReservaCriadaEvent evento)
        {
            await EnviarNotificacaoAsync("Sua reserva foi criada com sucesso!", evento.UsuarioId, Tipo.Email);
        }

        public async Task HandleAsync(ReservaAtualizadaEvent evento)
        {
            await EnviarNotificacaoAsync("Sua reserva foi atualizada com sucesso!", evento.UsuarioId, Tipo.Email);
        }

        public async Task HandleAsync(ReservaCanceladaEvent evento)
        {
            await EnviarNotificacaoAsync("Sua reserva foi cancelada com sucesso!", evento.UsuarioId, Tipo.Email);
        }

        private void EnviarSMS(Notificacao notificacao)
        {
            _logger.LogInformation("Enviando SMS para {Destinatario}: {Mensagem}", notificacao.Destinatario, notificacao.Mensagem);
        }

        private void EnviarPush(Notificacao notificacao)
        {
            _logger.LogInformation("Enviando Push notification para {Destinatario}: {Mensagem}", notificacao.Destinatario, notificacao.Mensagem);
        }

        //public void EnviarNotificacao(string mensagem, int usuarioId, Tipo tipo)
        //{
        //    throw new NotImplementedException();
        //}        
    }
}
