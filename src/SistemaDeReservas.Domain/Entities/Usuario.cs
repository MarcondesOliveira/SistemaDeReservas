using SistemaDeReservas.Domain.Enum;
using SistemaDeReservas.Domain.Inputs;

namespace SistemaDeReservas.Domain.Entities
{
    public class Usuario : Entity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public TipoPermissao Permissao { get; set; }
        public virtual ICollection<Reserva> Reservas { get; set; }

        public Usuario()
        {
        }

        public Usuario(CreateUsuarioInput input) 
        {
            Nome = input.Nome;
            Email = input.Email;
            Senha = input.Senha;
            Permissao = input.Permissao;
        }

        public Usuario(UpdateUsuarioInput Input)
        {
            Id = Input.Id;
            Nome = Input.Nome;
            Email = Input.Email;
            Senha = Input.Senha;
            Permissao = Input.Permissao;
        }
    }
}
