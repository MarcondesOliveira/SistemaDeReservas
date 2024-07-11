using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Inputs;
using SistemaDeReservas.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
