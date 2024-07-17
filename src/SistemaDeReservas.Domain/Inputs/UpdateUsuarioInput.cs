using SistemaDeReservas.Domain.Enum;

namespace SistemaDeReservas.Domain.Inputs
{
    public class UpdateUsuarioInput
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public required TipoPermissao Permissao { get; set; }
    }
}
