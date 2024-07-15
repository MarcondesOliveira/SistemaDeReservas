using Microsoft.EntityFrameworkCore;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Inputs;
using SistemaDeReservas.Domain.Repositories;

namespace SistemaDeReservas.Infrastructure.Persistence.Repositories
{
    public class UsuarioRepository : EFRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Usuario ObterPorNomeUsuarioESenha(string email, string senha)
        {
            var usuario = _context.Usuario
                .Include(c => c.Reservas)
                .FirstOrDefault(usuario =>
                usuario.Email == email && usuario.Senha == senha)
                    ?? throw new Exception("Esse usuário não existe");

            usuario.Reservas = usuario.Reservas
                .Where(c => c.Data >= DateTime.Now)
                .Select(p =>
                {
                    p.Usuario = null;

                    return p;
                })
                .ToList();

            return usuario;
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

        public async Task<IEnumerable<Usuario>> GetAllUsuarios()
        {
            return await _context.Usuario.ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> GetByUserId(int userId)
        {
            return await _context.Usuario.Where(r => r.Id == userId).ToListAsync();
        }
    }
}
