namespace SistemaDeReservas.Domain.Events
{
    public class ReservaAtualizadaEvent
    {
        public int ReservaId { get; set; }
        public int UsuarioId { get; set; }

        public ReservaAtualizadaEvent(int reservaId, int usuarioId)
        {
            ReservaId = reservaId;
            UsuarioId = usuarioId;
        }
    }

}
