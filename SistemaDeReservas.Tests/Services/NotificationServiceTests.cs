using Microsoft.Extensions.Logging;
using Moq;
using SistemaDeReservas.Application.Services;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Enum;
using SistemaDeReservas.Domain.Events;
using SistemaDeReservas.Domain.Repositories;

namespace SistemaDeReservas.Tests.Services
{
    public class NotificationServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ILogger<NotificationService>> _loggerMock;
        private readonly NotificationService _notificationService;

        public NotificationServiceTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _loggerMock = new Mock<ILogger<NotificationService>>();
            _notificationService = new NotificationService(_usuarioRepositoryMock.Object, _loggerMock.Object, _emailServiceMock.Object);
        }

        [Fact]
        public async Task EnviarNotificacaoAsync_ShouldSendEmailWhenTypeIsEmail()
        {
            // Arrange
            int usuarioId = 1;
            var usuario = new Usuario { Id = usuarioId, Email = "usuario@teste.com" };
            _usuarioRepositoryMock.Setup(r => r.GetById(usuarioId)).Returns(usuario);

            // Act
            await _notificationService.EnviarNotificacaoAsync("Mensagem de Teste", usuarioId, Tipo.Email);

            // Assert
            _emailServiceMock.Verify(e => e.SendEmailAsync(usuario.Email, "Notificação de Reserva", "Mensagem de Teste"), Times.Once);
        }

        [Fact]
        public async Task EnviarNotificacaoAsync_ShouldNotSendWhenUserNotFound()
        {
            // Arrange
            int usuarioId = 2;
            _usuarioRepositoryMock.Setup(r => r.GetById(usuarioId)).Returns((Usuario)null);

            // Act
            await _notificationService.EnviarNotificacaoAsync("Mensagem de Teste", usuarioId, Tipo.Email);

            // Assert
            _emailServiceMock.Verify(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Usuário não encontrado:")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ReservaCriadaEvent_ShouldSendReservationCreatedNotification()
        {
            // Arrange
            int usuarioId = 1;
            int reservaId = 1;
            var evento = new ReservaCriadaEvent(reservaId, usuarioId);
            var usuario = new Usuario { Id = usuarioId, Email = "usuario@teste.com" };
            _usuarioRepositoryMock.Setup(r => r.GetById(usuarioId)).Returns(usuario);

            // Act
            await _notificationService.HandleAsync(evento);

            // Assert
            _emailServiceMock.Verify(e => e.SendEmailAsync(usuario.Email, "Notificação de Reserva", "Sua reserva foi criada com sucesso!"), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ReservaAtualizadaEvent_ShouldSendReservationUpdatedNotification()
        {
            // Arrange
            int usuarioId = 1;
            int reservaId = 1;
            var evento = new ReservaAtualizadaEvent(reservaId, usuarioId);
            var usuario = new Usuario { Id = usuarioId, Email = "usuario@teste.com" };
            _usuarioRepositoryMock.Setup(r => r.GetById(usuarioId)).Returns(usuario);

            // Act
            await _notificationService.HandleAsync(evento);

            // Assert
            _emailServiceMock.Verify(e => e.SendEmailAsync(usuario.Email, "Notificação de Reserva", "Sua reserva foi atualizada com sucesso!"), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ReservaCanceladaEvent_ShouldSendReservationCancelledNotification()
        {
            // Arrange
            int usuarioId = 1;
            int reservaId = 1;
            var evento = new ReservaCanceladaEvent(reservaId, usuarioId);
            var usuario = new Usuario { Id = usuarioId, Email = "usuario@teste.com" };
            _usuarioRepositoryMock.Setup(r => r.GetById(usuarioId)).Returns(usuario);

            // Act
            await _notificationService.HandleAsync(evento);

            // Assert
            _emailServiceMock.Verify(e => e.SendEmailAsync(usuario.Email, "Notificação de Reserva", "Sua reserva foi cancelada com sucesso!"), Times.Once);
        }
    }
}
