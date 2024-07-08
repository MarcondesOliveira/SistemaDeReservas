using SistemaDeReservas.Domain.Enum;

namespace SistemaDeReservas.Domain.Entities
{
    public class Usuario : Entity
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public TipoPermissao Permissao { get; set; }
        public virtual ICollection<Reserva> Reservas { get; set; }

    }
}
