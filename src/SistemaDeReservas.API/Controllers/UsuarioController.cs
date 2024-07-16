using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDeReservas.API.Helpers;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Enum;
using SistemaDeReservas.Domain.Inputs;
using SistemaDeReservas.Domain.Repositories;
using System.Security.Claims;

namespace SistemaDeReservas.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _repository = usuarioRepository;
        }

        [HttpGet("obter-usuarios")]
        public async Task<ActionResult<IEnumerable<Usuario>>> ObterUsuarios()
        {
            try
            {
                var userId = User.FindFirst("Id")?.Value;
                var userPermissao = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userId == null || userPermissao == null)
                {
                    return Unauthorized();
                }

                IEnumerable<Usuario> usuarios;

                var isAdmin = userPermissao == Permissoes.Administrador;

                if (isAdmin)
                {
                    usuarios = await _repository.GetAllUsuarios();
                }
                else
                {
                    usuarios = await _repository.GetByUserId(int.Parse(userId));
                }

                var usuarioDto = usuarios.Select(r => r.ToDto()).ToList();

                return Ok(usuarioDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter usuários");
            }
        }

        [HttpGet("obter-usuario-por-id")]
        public IActionResult ObterUsuarioPorId(int id)
        {
            try
            {
                var userId = User.FindFirst("Id")?.Value;
                var userPermissao = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userId == null || userPermissao == null)
                {
                    return Unauthorized();
                }

                var usuario = _repository.GetById(id);
                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado");
                }

                var isAdmin = userPermissao == Permissoes.Administrador;
                var isOwnUser = usuario.Id.ToString() == userId;

                if (!isAdmin && !isOwnUser)
                {
                    return Unauthorized();
                }

                var usuarioDto = usuario.ToDto();

                return Ok(usuarioDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter usuário por id");
            }
        }

        [AllowAnonymous]
        [HttpPost("criar-usuario")]
        public IActionResult CriarUsuario([FromBody] CreateUsuarioInput usuario)
        {
            try
            {
                _repository.Create(usuario);

                return Ok("Usuario criado com sucesso");
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
                var userId = User.FindFirst("Id")?.Value;
                var userPermissao = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userId == null || userPermissao == null)
                {
                    return Unauthorized();
                }

                var isAdmin = userPermissao == Permissoes.Administrador;
                var isOwnUser = usuario.Id.ToString() == userId;


                if (!isAdmin && !isOwnUser)
                {
                    return Unauthorized();
                }

                _repository.Update(usuario);

                return Ok("Usuario alterado com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao alterar usuário");
            }
        }

        [Authorize]
        [Authorize(Roles = Permissoes.Administrador)]
        [HttpDelete("{id}")]
        public IActionResult DeletarUsuario(int id)
        {
            try
            {
                var userId = User.FindFirst("Id")?.Value;
                var userPermissao = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userId == null || userPermissao == null || userPermissao != Permissoes.Administrador)
                {
                    return Unauthorized();
                }

                var usuario = _repository.GetById(id);
                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado");
                }

                _repository.Delete(id);
                return Ok($"Usuário deletado com sucesso | Id: {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao deletar usuário");
            }
        }
    }
}
