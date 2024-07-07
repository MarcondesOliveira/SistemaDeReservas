using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Infrastructure.Persistence.Repositories
{
    public class UsuarioRepository : EFRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
