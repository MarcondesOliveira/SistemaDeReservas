using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SistemaDeReservas.API.Controllers;
using SistemaDeReservas.Application.DTOs;
using SistemaDeReservas.Application.Services;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Enum;
using SistemaDeReservas.Domain.Inputs;
using SistemaDeReservas.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Tests.Controllers
{
    public class ReservaControllerTests
    {
        private readonly Mock<IReservaRepository> _repositoryMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<ILogger<EmailService>> _loggerMock;
        private readonly ReservaController _controller;

        public ReservaControllerTests()
        {
            _repositoryMock = new Mock<IReservaRepository>();
            _notificationServiceMock = new Mock<INotificationService>();
            _loggerMock = new Mock<ILogger<EmailService>>();
            _controller = new ReservaController(_repositoryMock.Object, _notificationServiceMock.Object, _loggerMock.Object);
        }

        private void SetupUserClaims(string userId, string role)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", userId),
                new Claim(ClaimTypes.Role, role)
            }, "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task ObterReservas_AdminUser_ReturnsAllReservas()
        {
            // Arrange
            SetupUserClaims("1", Permissoes.Administrador);
            var reservas = new List<Reserva> { new Reserva { Id = 1 } };
            _repositoryMock.Setup(r => r.GetAllReservas()).ReturnsAsync(reservas);

            // Act
            var result = await _controller.ObterReservas();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ReservaDto>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal(1, returnValue.First().Id);
        }

        [Fact]
        public async Task ObterReservas_RegularUser_ReturnsUserReservas()
        {
            // Arrange
            SetupUserClaims("1", Permissoes.User);
            var reservas = new List<Reserva> { new Reserva { Id = 1, UsuarioId = 1 } };
            _repositoryMock.Setup(r => r.GetByUserId(1)).ReturnsAsync(reservas);

            // Act
            var result = await _controller.ObterReservas();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ReservaDto>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal(1, returnValue.First().Id);
        }

        [Fact]
        public async Task ObterReservas_UnauthorizedUser_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await _controller.ObterReservas();

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public void ObterReservaPorId_ValidId_ReturnsReserva()
        {
            // Arrange
            SetupUserClaims("1", Permissoes.Administrador);
            var reserva = new Reserva { Id = 1, UsuarioId = 1 };
            _repositoryMock.Setup(r => r.GetById(1)).Returns(reserva);

            // Act
            var result = _controller.ObterReservaPorId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ReservaDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public void ObterReservaPorId_InvalidId_ReturnsNotFound()
        {
            // Arrange
            SetupUserClaims("1", Permissoes.Administrador);
            _repositoryMock.Setup(r => r.GetById(1)).Returns((Reserva)null);

            // Act
            var result = _controller.ObterReservaPorId(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CriarReserva_ValidInput_ReturnsOk()
        {
            // Arrange
            SetupUserClaims("1", Permissoes.Administrador);
            var reservaInput = new CreateReservaInput { Data = System.DateTime.Now, Hora = "10:00", Status = Status.Pendente, UsuarioId = 1 };

            // Act
            var result = _controller.CriarReserva(reservaInput);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void CriarReserva_UnauthorizedUser_ReturnsUnauthorized()
        {
            // Arrange
            SetupUserClaims("2", Permissoes.User);
            var reservaInput = new CreateReservaInput { Data = System.DateTime.Now, Hora = "10:00", Status = Status.Pendente, UsuarioId = 1 };

            // Act
            var result = _controller.CriarReserva(reservaInput);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void AlterarReserva_ValidInput_ReturnsOk()
        {
            // Arrange
            SetupUserClaims("1", Permissoes.Administrador);
            var reservaInput = new UpdateReservaInput { Id = 1, Data = System.DateTime.Now, Hora = "11:00", Status = Status.Confirmada, UsuarioId = 1 };
            var reserva = new Reserva { Id = 1, UsuarioId = 1 };
            _repositoryMock.Setup(r => r.GetById(1)).Returns(reserva);

            // Act
            var result = _controller.AlterarReserva(reservaInput);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void AlterarReserva_InvalidId_ReturnsNotFound()
        {
            // Arrange
            SetupUserClaims("1", Permissoes.Administrador);
            var reservaInput = new UpdateReservaInput { Id = 1, Data = System.DateTime.Now, Hora = "11:00", Status = Status.Confirmada, UsuarioId = 1 };
            _repositoryMock.Setup(r => r.GetById(1)).Returns((Reserva)null);

            // Act
            var result = _controller.AlterarReserva(reservaInput);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void AlterarReserva_UnauthorizedUser_ReturnsUnauthorized()
        {
            // Arrange
            SetupUserClaims("2", Permissoes.User);
            var reservaInput = new UpdateReservaInput { Id = 1, Data = System.DateTime.Now, Hora = "11:00", Status = Status.Confirmada, UsuarioId = 1 };
            var reserva = new Reserva { Id = 1, UsuarioId = 1 };
            _repositoryMock.Setup(r => r.GetById(1)).Returns(reserva);

            // Act
            var result = _controller.AlterarReserva(reservaInput);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void CancelarReserva_ValidId_ReturnsOk()
        {
            // Arrange
            SetupUserClaims("1", Permissoes.Administrador);
            var reserva = new Reserva { Id = 1, UsuarioId = 1 };
            _repositoryMock.Setup(r => r.GetById(1)).Returns(reserva);

            // Act
            var result = _controller.CancelarReserva(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void CancelarReserva_InvalidId_ReturnsNotFound()
        {
            // Arrange
            SetupUserClaims("1", Permissoes.Administrador);
            _repositoryMock.Setup(r => r.GetById(1)).Returns((Reserva)null);

            // Act
            var result = _controller.CancelarReserva(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CancelarReserva_UnauthorizedUser_ReturnsUnauthorized()
        {
            // Arrange
            SetupUserClaims("2", Permissoes.User);
            var reserva = new Reserva { Id = 1, UsuarioId = 1 };
            _repositoryMock.Setup(r => r.GetById(1)).Returns(reserva);

            // Act
            var result = _controller.CancelarReserva(1);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

    }

}