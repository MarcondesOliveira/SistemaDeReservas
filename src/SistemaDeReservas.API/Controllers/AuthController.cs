using Microsoft.AspNetCore.Mvc;
using SistemaDeReservas.Application.DTOs;
using SistemaDeReservas.Domain.Repositories;

namespace SistemaDeReservas.API.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public AuthController(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        [HttpPost]
        public IActionResult Autenticar([FromBody] LoginDto usuarioDto)
        {
            var usuario = _usuarioRepository.ObterPorNomeUsuarioESenha(usuarioDto.Email, usuarioDto.Senha);

            if (usuario == null)
            {
                return NotFound(new { mensagem = "Usuário ou senha inválidos" });
            }

            var token = _tokenService.GerarToken(usuario);

            usuario.Senha = null;

            return Ok(new
            {
                Token = token
            });
        }
    }
}
