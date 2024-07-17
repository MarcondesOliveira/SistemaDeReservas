﻿using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Inputs;

namespace SistemaDeReservas.Domain.Repositories
{
    public interface IReservaRepository : IRepository<Reserva>
    {
        void Create(CreateReservaInput reserva);
        void Update(UpdateReservaInput reserva);
        Task<IEnumerable<Reserva>> GetAllReservas();
        Task<IEnumerable<Reserva>> GetByUserId(int userId);
    }
}
