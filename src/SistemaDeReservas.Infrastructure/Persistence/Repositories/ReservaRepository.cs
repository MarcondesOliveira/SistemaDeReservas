using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Inputs;
using SistemaDeReservas.Domain.Repositories;
using SistemaDeReservas.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Infrastructure.Persistence.Repositories
{
    public class ReservaRepository : EFRepository<Reserva>, IReservaRepository
    {
        public ReservaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void Create(CreateReservaInput reserva)
        {
            _dbSet.Add(new Reserva(reserva));
            _context.SaveChanges();
        }

        public void Update(UpdateReservaInput reserva)
        {
            _dbSet.Update(new Reserva(reserva));
            _context.SaveChanges();
        }
    }
}
