using SistemaDeReservas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Domain.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        Task<IEnumerable<T>> GetAll();
        T GetById(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
