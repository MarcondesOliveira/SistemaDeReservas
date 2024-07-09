using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Domain.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        void Create(CreateUsuarioInput usuario);
        void Update(UpdateUsuarioInput usuario);
        Usuario ObterPorNomeUsuarioESenha(string email, string senha);
    }
}
