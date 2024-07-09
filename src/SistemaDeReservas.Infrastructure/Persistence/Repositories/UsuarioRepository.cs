using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Inputs;
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

        public Usuario ObterPorNomeUsuarioESenha(string email, string senha)
        {
            return _context.Usuario.FirstOrDefault(usuario =>
                usuario.Email == email && usuario.Senha == senha);
        }

        public void Create(CreateUsuarioInput usuario)
        {
            _dbSet.Add(new Usuario(usuario));
            _context.SaveChanges();
        }

        public void Update(UpdateUsuarioInput usuario)
        {
            _dbSet.Update(new Usuario(usuario));
            _context.SaveChanges();
        }
    }
}
