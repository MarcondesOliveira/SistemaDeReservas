using SistemaDeReservas.Domain.Events;

namespace SistemaDeReservas.Domain.Repositories
{
    public interface INotificationService
    {
        Task HandleAsync(ReservaCriadaEvent evento);

        Task HandleAsync(ReservaAtualizadaEvent evento);

        Task HandleAsync(ReservaCanceladaEvent evento);
    }
}
