using Moq;
using SistemaDeReservas.Application.Services;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Enum;
using SistemaDeReservas.Domain.Inputs;
using SistemaDeReservas.Domain.Repositories;

namespace SistemaDeReservas.Tests.Services
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllUsers()
        {
            // Arrange
            var usuarios = new List<Usuario>
            {
                new() { Id = 1, Nome = "Usuario 1" },
                new() { Id = 2, Nome = "Usuario 2" }
            };

            _usuarioRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(usuarios);

            // Act
            var result = await _usuarioService.GetAll();

            // Assert
            Assert.Equal(usuarios, result);
        }

        [Fact]
        public void Create_ShouldAddUserToRepository()
        {
            // Arrange
            var input = new CreateUsuarioInput
            {
                Nome = "Novo Usuario",
                Email = "novo@usuario.com",
                Senha = "senha123",
                Permissao = TipoPermissao.User
            };

            // Act
            _usuarioService.Create(input);

            // Assert
            _usuarioRepositoryMock.Verify(r => r.Create(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public void ObterPorId_ShouldReturnUserById()
        {
            // Arrange
            var usuarioId = 1;
            var usuario = new Usuario { Id = usuarioId, Nome = "Usuario 1" };

            _usuarioRepositoryMock.Setup(r => r.GetById(usuarioId)).Returns(usuario);

            // Act
            var result = _usuarioService.ObterPorId(usuarioId);

            // Assert
            Assert.Equal(usuario, result);
        }

        [Fact]
        public void Update_ShouldUpdateUserInRepository()
        {
            // Arrange
            var input = new UpdateUsuarioInput
            {
                Id = 1,
                Nome = "Usuario Atualizado",
                Email = "atualizado@usuario.com",
                Senha = "novaSenha123",
                Permissao = TipoPermissao.Administrador
            };

            // Act
            _usuarioService.Update(input);

            // Assert
            _usuarioRepositoryMock.Verify(r => r.Update(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldRemoveUserFromRepository()
        {
            // Arrange
            int usuarioId = 1;

            // Act
            _usuarioService.Delete(usuarioId);

            // Assert
            _usuarioRepositoryMock.Verify(r => r.Delete(usuarioId), Times.Once);
        }
    }

}
