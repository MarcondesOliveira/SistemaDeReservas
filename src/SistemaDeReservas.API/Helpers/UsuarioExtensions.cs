using SistemaDeReservas.Application.DTOs;
using SistemaDeReservas.Domain.Entities;

namespace SistemaDeReservas.API.Helpers
{
    public static class UsuarioExtensions
    {
        public static UsuarioDto ToDto(this Usuario usuario)
        {
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Permissao = usuario.Permissao
            };
        }
    }
}