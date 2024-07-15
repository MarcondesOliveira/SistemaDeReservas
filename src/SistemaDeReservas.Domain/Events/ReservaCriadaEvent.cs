namespace SistemaDeReservas.Domain.Events
{
    public class ReservaCriadaEvent
    {
        public int ReservaId { get; set; }
        public int UsuarioId { get; set; }

        public ReservaCriadaEvent(int reservaId, int usuarioId)
        {
            ReservaId = reservaId;
            UsuarioId = usuarioId;
        }
    }

}
