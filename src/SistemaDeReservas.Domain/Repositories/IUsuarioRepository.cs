using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Inputs;

namespace SistemaDeReservas.Domain.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        void Create(CreateUsuarioInput usuario);
        void Update(UpdateUsuarioInput usuario);
        Task<IEnumerable<Usuario>> GetAllUsuarios();
        Task<IEnumerable<Usuario>> GetByUserId(int userId);
        Usuario ObterPorNomeUsuarioESenha(string email, string senha);
    }
}
