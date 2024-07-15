using SistemaDeReservas.Domain.Entities;

namespace SistemaDeReservas.Domain.Repositories
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
    }
}
