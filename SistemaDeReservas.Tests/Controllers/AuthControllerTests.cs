using Microsoft.AspNetCore.Mvc;
using Moq;
using SistemaDeReservas.API.Controllers;
using SistemaDeReservas.Application.DTOs;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _controller = new AuthController(_usuarioRepositoryMock.Object, _tokenServiceMock.Object);
        }

        [Fact]
        public void Autenticar_ValidCredentials_ReturnsOkResultWithToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "valid@example.com",
                Senha = "validpassword"
            };

            var usuario = new Usuario
            {
                Id = 1,
                Nome = "Valid User",
                Email = "valid@example.com",
                Senha = "validpassword",
                Permissao = Domain.Enum.TipoPermissao.User
            };

            var token = "generated_token";

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorNomeUsuarioESenha(loginDto.Email, loginDto.Senha))
                                  .Returns(usuario);

            _tokenServiceMock.Setup(service => service.GerarToken(usuario))
                             .Returns(token);

            // Act
            var result = _controller.Autenticar(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value;

            Assert.NotNull(returnValue);
            Assert.Equal(token, returnValue.GetType().GetProperty("Token").GetValue(returnValue, null));
        }

        [Fact]
        public void Autenticar_InvalidCredentials_ReturnsNotFoundResult()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "invalid@example.com",
                Senha = "invalidpassword"
            };

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorNomeUsuarioESenha(loginDto.Email, loginDto.Senha))
                                  .Returns((Usuario)null);

            // Act
            var result = _controller.Autenticar(loginDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = notFoundResult.Value;

            Assert.NotNull(returnValue);
            Assert.Equal("Usuário ou senha inválidos", returnValue.GetType().GetProperty("mensagem").GetValue(returnValue, null));
        }
    }

}
