using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Reserva>> GetAllReservas()
        {
            return await _context.Reserva.ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> GetByUserId(int userId)
        {
            return await _context.Reserva.Where(r => r.UsuarioId == userId).ToListAsync();
        }

        public void Update(UpdateReservaInput reserva)
        {
            _dbSet.Update(new Reserva(reserva));
            _context.SaveChanges();
        }        
    }
}
