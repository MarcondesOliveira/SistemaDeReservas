namespace SistemaDeReservas.Domain.Enum
{
    public enum TipoPermissao
    {
        Administrador = 1,
        User = 2
    }

    public static class Permissoes
    {
        public const string Administrador = "Administrador";
        public const string User = "User";
    }
}
