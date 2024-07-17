using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SistemaDeReservas.API.Controllers;
using SistemaDeReservas.Application.DTOs;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Enum;
using SistemaDeReservas.Domain.Inputs;
using SistemaDeReservas.Domain.Repositories;
using System.Security.Claims;

namespace SistemaDeReservas.Tests.Controllers
{
    public class UsuarioControllerTests
    {
        private readonly Mock<IUsuarioRepository> _repositoryMock;
        private readonly UsuarioController _controller;

        public UsuarioControllerTests()
        {
            _repositoryMock = new Mock<IUsuarioRepository>();
            _controller = new UsuarioController(_repositoryMock.Object);
        }

        private void SetupUserClaims(string userId, TipoPermissao role)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", userId),
                new Claim(ClaimTypes.Role, role.ToString())
            }, "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task ObterUsuarios_AdminUser_ReturnsAllUsuarios()
        {
            // Arrange
            SetupUserClaims("1", TipoPermissao.Administrador);
            var usuarios = new List<Usuario>
            {
                new Usuario { Id = 1, Nome = "Teste", Email = "teste@example.com", Senha = "1234", Permissao = TipoPermissao.Administrador }
            };
            _repositoryMock.Setup(r => r.GetAllUsuarios()).ReturnsAsync(usuarios);

            // Act
            var result = await _controller.ObterUsuarios();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<UsuarioDto>>(okResult.Value);
            Assert.Single(returnValue);
            var usuarioDto = returnValue.First();
            Assert.Equal(1, usuarioDto.Id);
            Assert.Equal("Teste", usuarioDto.Nome);
            Assert.Equal("teste@example.com", usuarioDto.Email);
            Assert.Equal("1234", usuarioDto.Senha);
            Assert.Equal(TipoPermissao.Administrador, usuarioDto.Permissao);
        }

        [Fact]
        public async Task ObterUsuarios_RegularUser_ReturnsUserUsuarios()
        {
            // Arrange
            SetupUserClaims("1", TipoPermissao.User);
            var usuarios = new List<Usuario> { new Usuario { Id = 1 } };
            _repositoryMock.Setup(r => r.GetByUserId(1)).ReturnsAsync(usuarios);

            // Act
            var result = await _controller.ObterUsuarios();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<UsuarioDto>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal(1, returnValue.First().Id);
        }

        [Fact]
        public async Task ObterUsuarios_UnauthorizedUser_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await _controller.ObterUsuarios();

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public void ObterUsuarioPorId_ValidId_ReturnsUsuario()
        {
            // Arrange
            SetupUserClaims("1", TipoPermissao.Administrador);
            var usuario = new Usuario { Id = 1 };
            _repositoryMock.Setup(r => r.GetById(1)).Returns(usuario);

            // Act
            var result = _controller.ObterUsuarioPorId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UsuarioDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public void ObterUsuarioPorId_InvalidId_ReturnsNotFound()
        {
            // Arrange
            SetupUserClaims("1", TipoPermissao.Administrador);
            _repositoryMock.Setup(r => r.GetById(1)).Returns((Usuario)null);

            // Act
            var result = _controller.ObterUsuarioPorId(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CriarUsuario_ValidInput_ReturnsOk()
        {
            // Arrange
            var usuarioInput = new CreateUsuarioInput { Nome = "Test User", Email = "test@example.com", Senha = "password", Permissao = TipoPermissao.User };

            // Act
            var result = _controller.CriarUsuario(usuarioInput);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AlterarUsuario_ValidInput_ReturnsOk()
        {
            // Arrange
            SetupUserClaims("1", TipoPermissao.Administrador);
            var usuarioInput = new UpdateUsuarioInput { Id = 1, Nome = "Updated User", Email = "updated@example.com", Senha = "newpassword", Permissao = TipoPermissao.User };
            var usuario = new Usuario { Id = 1 };
            _repositoryMock.Setup(r => r.GetById(1)).Returns(usuario);

            // Act
            var result = _controller.AlterarUsuario(usuarioInput);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }               

        [Fact]
        public void AlterarUsuario_UnauthorizedUser_ReturnsUnauthorized()
        {
            // Arrange
            SetupUserClaims("2", TipoPermissao.User);
            var usuarioInput = new UpdateUsuarioInput { Id = 1, Nome = "Updated User", Email = "updated@example.com", Senha = "newpassword", Permissao = TipoPermissao.User };
            var usuario = new Usuario { Id = 1 };
            _repositoryMock.Setup(r => r.GetById(1)).Returns(usuario);

            // Act
            var result = _controller.AlterarUsuario(usuarioInput);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void DeletarUsuario_ValidId_ReturnsOk()
        {
            // Arrange
            SetupUserClaims("1", TipoPermissao.Administrador);
            var usuario = new Usuario { Id = 1 };
            _repositoryMock.Setup(r => r.GetById(1)).Returns(usuario);

            // Act
            var result = _controller.DeletarUsuario(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeletarUsuario_InvalidId_ReturnsNotFound()
        {
            // Arrange
            SetupUserClaims("1", TipoPermissao.Administrador);
            _repositoryMock.Setup(r => r.GetById(1)).Returns((Usuario)null);

            // Act
            var result = _controller.DeletarUsuario(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeletarUsuario_UnauthorizedUser_ReturnsUnauthorized()
        {
            // Arrange
            SetupUserClaims("2", TipoPermissao.User); // Não é um administrador
            var usuario = new Usuario { Id = 1 };
            _repositoryMock.Setup(r => r.GetById(1)).Returns(usuario);

            // Act
            var result = _controller.DeletarUsuario(1);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}


