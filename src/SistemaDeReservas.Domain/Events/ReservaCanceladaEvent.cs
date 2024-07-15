namespace SistemaDeReservas.Domain.Events
{
    public class ReservaCanceladaEvent
    {
        public int ReservaId { get; set; }
        public int UsuarioId { get; set; }

        public ReservaCanceladaEvent(int reservaId, int usuarioId)
        {
            ReservaId = reservaId;
            UsuarioId = usuarioId;
        }
    }

}
