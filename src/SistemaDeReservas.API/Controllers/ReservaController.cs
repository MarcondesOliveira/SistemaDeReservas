using Microsoft.AspNetCore.Mvc;
using SistemaDeReservas.Domain.Inputs;
using SistemaDeReservas.Domain.Repositories;

namespace SistemaDeReservas.API.Controllers
{
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
                _repository.Create(reserva);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
