using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDeReservas.Application.DTOs;
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
        [HttpGet("obter-usuarios")]
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
        [Authorize]
        [Authorize(Roles = Permissoes.Administrador)]
        [HttpGet("obter-usuario-por-id")]
        public IActionResult ObterUsuarioPorId(int id)
        {
            try
            {
                var usuario = _usuarioRepository.GetById(id);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter usuário por id");
            }
        }

        [HttpPost("criar-usuario")]
        public IActionResult CriarUsuario([FromBody] CreateUsuarioInput usuario) 
        {
            try
            {
                _usuarioRepository.Create(usuario);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
                
        [HttpPut("alterar-usuario")]
        public IActionResult AlterarUsuario(UpdateUsuarioInput usuario)
        {
            try
            {
                _usuarioRepository.Update(usuario);
                return Ok("Usuario alterado com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao alterar usuário");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarUsuario(int id)
        {
            try
            {
                _usuarioRepository.Delete(id);
                return Ok($"Usuário deletado com sucesso | Id: {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao deletar usuário");
            }
        }
    }
}
