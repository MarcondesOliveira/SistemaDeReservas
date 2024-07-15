using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Inputs;
using SistemaDeReservas.Domain.Repositories;

namespace SistemaDeReservas.Application.Services
{
    public class ReservaService
    {
        private readonly IReservaRepository _repository;

        public ReservaService(IReservaRepository repository)
        {
            _repository = repository;
        }

        public void Create(CreateReservaInput input)
        {
            var reserva = new Reserva(input);

            _repository.Create(reserva);
        }

        public void Update(UpdateReservaInput input)
        {
            var reserva = new Reserva(input);

            _repository.Update(reserva);
        }

        public Task<IEnumerable<Reserva>> GetAllReservas()
        {
            return _repository.GetAllReservas();
        }

        public Task<IEnumerable<Reserva>> GetByUserId(int userId)
        {
            return _repository.GetByUserId(userId);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
