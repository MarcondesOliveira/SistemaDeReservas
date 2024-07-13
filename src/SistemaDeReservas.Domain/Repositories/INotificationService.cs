using SistemaDeReservas.Domain.Enum;
using SistemaDeReservas.Domain.Events;

namespace SistemaDeReservas.Domain.Repositories
{
    public interface INotificationService
    {
        void EnviarNotificacao(string mensagem, int usuarioId, Tipo tipo);
        void Handle(ReservaCriadaEvent evento);
        void Handle(ReservaAtualizadaEvent evento);
        void Handle(ReservaCanceladaEvent evento);
    }
}
