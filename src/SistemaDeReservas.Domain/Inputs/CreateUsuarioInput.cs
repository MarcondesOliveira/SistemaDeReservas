using SistemaDeReservas.Domain.Enum;

namespace SistemaDeReservas.Domain.Inputs
{
    public class CreateUsuarioInput
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public TipoPermissao Permissao { get; set; }
    }
}
