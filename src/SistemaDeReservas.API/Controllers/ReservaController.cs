using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDeReservas.Domain.Enum;
using SistemaDeReservas.Domain.Inputs;
using SistemaDeReservas.Domain.Repositories;
using SistemaDeReservas.Infrastructure.Persistence.Repositories;
using System.Security.Claims;

namespace SistemaDeReservas.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaRepository _repository;

        public ReservaController(IReservaRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("criar-reserva")]
        public IActionResult CriarReserva([FromBody] CreateReservaInput reserva)
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
                var isOwnUser = reserva.UsuarioId.ToString() == userId;

                if (!isAdmin && !isOwnUser)
                {
                    return Unauthorized();
                }

                _repository.Create(reserva);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("alterar-reserva")]
        public IActionResult AlterarReserva(UpdateReservaInput reserva)
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
                var isOwnUser = reserva.UsuarioId.ToString() == userId;

                if (!isAdmin && !isOwnUser)
                {
                    return Unauthorized();
                }

                _repository.Update(reserva);
                return Ok("Reserva alterada com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao alterar reserva");
            }
        }
    }
}
