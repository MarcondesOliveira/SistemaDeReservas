using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDeReservas.Application.Services;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Enum;
using SistemaDeReservas.Domain.Inputs;
using SistemaDeReservas.Domain.Repositories;

namespace SistemaDeReservas.API.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [Authorize]
        [Authorize(Roles = Permissoes.Administrador)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> ObterUsuarios()
        {
            try
            {
                var usuario = await _usuarioRepository.GetAll();
                return Ok(usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public IActionResult CriarUsuario([FromBody] UsuarioInput input) 
        {
            try
            {
                var usuario = new Usuario()
                {
                    Nome = input.Nome,
                    Email = input.Email,
                    Senha = input.Senha
                };

                _usuarioRepository.Create(usuario);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
