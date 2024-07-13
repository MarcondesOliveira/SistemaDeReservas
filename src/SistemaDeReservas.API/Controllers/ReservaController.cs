using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDeReservas.Application.Helpers;
using SistemaDeReservas.Application.Services;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Enum;
using SistemaDeReservas.Domain.Events;
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
        private readonly INotificationService _notificationService;
        private readonly ILogger<EmailService> _logger;

        public ReservaController(IReservaRepository repository, INotificationService notificationService, ILogger<EmailService> logger)
        {
            _repository = repository;
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet("obter-reservas")]
        public async Task<ActionResult<IEnumerable<Reserva>>> ObterReservas()
        {
            try
            {
                var userId = User.FindFirst("Id")?.Value;
                var userPermissao = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userId == null || userPermissao == null)
                {
                    return Unauthorized();
                }

                IEnumerable<Reserva> reservas;

                var isAdmin = userPermissao == Permissoes.Administrador;

                if (isAdmin)
                {
                    reservas = await _repository.GetAllReservas();
                }
                else
                {
                    reservas = await _repository.GetByUserId(int.Parse(userId));
                }

                var reservaDtos = reservas.Select(r => r.ToDto()).ToList();

                return Ok(reservaDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter reservas");
            }
        }

        [HttpGet("obter-reserva-por-id")]
        public IActionResult ObterReservaPorId(int id)
        {
            try
            {
                var userId = User.FindFirst("Id")?.Value;
                var userPermissao = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userId == null || userPermissao == null)
                {
                    return Unauthorized();
                }

                var reserva = _repository.GetById(id);
                if (reserva == null)
                {
                    return NotFound("Reserva não encontrada");
                }

                var isAdmin = userPermissao == Permissoes.Administrador;
                var isOwnUser = reserva.UsuarioId.ToString() == userId;

                if (!isAdmin && !isOwnUser)
                {
                    return Unauthorized();
                }

                var reservaDto = reserva.ToDto();
                                
                return Ok(reservaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter reserva por id");
            }
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
                _notificationService.HandleAsync(new ReservaCriadaEvent(reserva.Id, reserva.UsuarioId));
                
                _logger.LogInformation("Reserva criada e email enviado.");


                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("alterar-reserva")] // TODO: ajustar
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
                _notificationService.HandleAsync(new ReservaAtualizadaEvent(reserva.Id, reserva.UsuarioId));

                _logger.LogInformation("Reserva alterada e email enviado.");

                return Ok("Reserva alterada com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao alterar reserva");
            }
        }



        [HttpDelete("{id}")]
        public IActionResult CancelarReserva(int id)
        {
            try
            {
                var userId = User.FindFirst("Id")?.Value;
                var userPermissao = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userId == null || userPermissao == null)
                {
                    return Unauthorized();
                }

                var reserva = _repository.GetById(id);
                if (reserva == null)
                {
                    return NotFound("Reserva não encontrada");
                }

                var isAdmin = userPermissao == Permissoes.Administrador;
                var isOwnUser = reserva.UsuarioId.ToString() == userId;

                if (!isAdmin && !isOwnUser)
                {
                    return Unauthorized();
                }

                _repository.Delete(id);
                _notificationService.HandleAsync(new ReservaCanceladaEvent(reserva.Id, reserva.UsuarioId));

                _logger.LogInformation("Reserva cancelada e email enviado.");

                return Ok($"Reserva cancelada com sucesso | Id: {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao cancelar reserva");
            }
        }
    }
}
