using Microsoft.EntityFrameworkCore;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Infrastructure.Persistence.Repositories
{
    public class EFRepository<T> : IRepository<T> where T : Entity
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> _dbSet;

        public EFRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _dbSet.Remove(GetById(id));
            _context.SaveChanges();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public T GetById(int id)
        {
            return _dbSet.SingleOrDefault(x => x.Id == id)
                ?? throw new Exception("Esse usuário não existe");
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }
    }
}
