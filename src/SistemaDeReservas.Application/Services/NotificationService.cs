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
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IUsuarioRepository usuarioRepository, ILogger<NotificationService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public void EnviarNotificacao(string mensagem, int usuarioId, Tipo tipo)
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
                        EnviarEmail(notificacao);
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

        public void Handle(ReservaCriadaEvent evento)
        {
            EnviarNotificacao("Sua reserva foi criada com sucesso!", evento.UsuarioId, Tipo.Email);
        }

        public void Handle(ReservaAtualizadaEvent evento)
        {
            EnviarNotificacao("Sua reserva foi atualizada com sucesso!", evento.UsuarioId, Tipo.Email);
        }

        public void Handle(ReservaCanceladaEvent evento)
        {
            EnviarNotificacao("Sua reserva foi cancelada com sucesso!", evento.UsuarioId, Tipo.Email);
        }

        private void EnviarEmail(Notificacao notificacao)
        {
            _logger.LogInformation("Enviando e-mail para {Destinatario}: {Mensagem}", notificacao.Destinatario, notificacao.Mensagem);
        }

        private void EnviarSMS(Notificacao notificacao)
        {
            _logger.LogInformation("Enviando SMS para {Destinatario}: {Mensagem}", notificacao.Destinatario, notificacao.Mensagem);
        }

        private void EnviarPush(Notificacao notificacao)
        {
            _logger.LogInformation("Enviando Push notification para {Destinatario}: {Mensagem}", notificacao.Destinatario, notificacao.Mensagem);
        }
    }
}
