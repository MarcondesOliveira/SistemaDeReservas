using Moq;
using SistemaDeReservas.Application.Services;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Repositories;
using SistemaDeReservas.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Tests.Services
{
    public class ReservaServiceTests
    {
        private readonly Mock<IRepository<Reserva>> _reservaRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly ReservaService _reservaService;

        public ReservaServiceTests()
        {
            _reservaRepositoryMock = new Mock<IRepository<Reserva>>();
            _emailServiceMock = new Mock<IEmailService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _reservaService = new ReservaService(_reservaRepositoryMock.Object, _emailServiceMock.Object, _notificationServiceMock.Object);
        }

        [Fact]
        public async Task CriarReserva_DeveDispararEventoDeReservaCriada()
        {
            // Arrange
            var reserva = new Reserva { Id = 1, UsuarioId = 1, Data = DateTime.Now, Hora = TimeSpan.FromHours(12), Status = Status.Pendente };

            // Act
            await _reservaService.CriarReserva(reserva);

            // Assert
            _reservaRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Reserva>()), Times.Once);
            _notificationServiceMock.Verify(n => n.HandleAsync(It.IsAny<ReservaCriadaEvent>()), Times.Once);
        }

    }
}
