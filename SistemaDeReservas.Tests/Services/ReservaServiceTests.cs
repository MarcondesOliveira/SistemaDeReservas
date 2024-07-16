using Moq;
using SistemaDeReservas.Application.Services;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Enum;
using SistemaDeReservas.Domain.Inputs;
using SistemaDeReservas.Domain.Repositories;

namespace SistemaDeReservas.Tests.Services
{
    public class ReservaServiceTests
    {
        private readonly Mock<IReservaRepository> _reservaRepositoryMock;
        private readonly ReservaService _reservaService;

        public ReservaServiceTests()
        {
            _reservaRepositoryMock = new Mock<IReservaRepository>();
            _reservaService = new ReservaService(_reservaRepositoryMock.Object);
        }

        [Fact]
        public void Create_ShouldAddReservationToRepository()
        {
            // Arrange
            var input = new CreateReservaInput
            {
                Data = DateTime.Now.AddDays(6),
                Hora = "18:00",
                Status = Status.Pendente,
                UsuarioId = 1
            };

            // Act
            _reservaService.Create(input);

            // Assert
            _reservaRepositoryMock.Verify(r => r.Create(It.IsAny<Reserva>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldUpdateReservationInRepository()
        {
            // Arrange
            var input = new UpdateReservaInput
            {
                Id = 1,
                Data = DateTime.Now.AddDays(6),
                Hora = "19:00",
                Status = Status.Pendente,
                UsuarioId = 1
            };

            // Act
            _reservaService.Update(input);

            // Assert
            _reservaRepositoryMock.Verify(r => r.Update(It.IsAny<Reserva>()), Times.Once);
        }

        [Fact]
        public async Task GetAllReservas_ShouldReturnAllReservations()
        {
            // Arrange
            var reservas = new List<Reserva>
            {
                new Reserva { Id = 1, UsuarioId = 1, Data = DateTime.Now.AddDays(6), Status = Status.Pendente },
                new Reserva { Id = 2, UsuarioId = 2, Data = DateTime.Now.AddDays(6), Status = Status.Pendente }
            };

            _reservaRepositoryMock.Setup(r => r.GetAllReservas()).ReturnsAsync(reservas);

            // Act
            var result = await _reservaService.GetAllReservas();

            // Assert
            Assert.Equal(reservas, result);
        }

        [Fact]
        public async Task GetByUserId_ShouldReturnUserReservations()
        {
            // Arrange
            int userId = 1;
            var reservas = new List<Reserva>
            {
                new Reserva { Id = 1, UsuarioId = 1, Data = DateTime.Now.AddDays(6), Status = Status.Pendente },
                new Reserva { Id = 2, UsuarioId = 2, Data = DateTime.Now.AddDays(6), Status = Status.Pendente }
            };

            _reservaRepositoryMock.Setup(r => r.GetByUserId(userId)).ReturnsAsync(reservas);

            // Act
            var result = await _reservaService.GetByUserId(userId);

            // Assert
            Assert.Equal(reservas, result);
        }

        [Fact]
        public void Delete_ShouldRemoveReservationFromRepository()
        {
            // Arrange
            int reservaId = 1;

            // Act
            _reservaService.Delete(reservaId);

            // Assert
            _reservaRepositoryMock.Verify(r => r.Delete(reservaId), Times.Once);
        }
    }
}