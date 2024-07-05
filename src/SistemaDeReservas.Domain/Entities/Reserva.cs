using SistemaDeReservas.Domain.Enum;

namespace SistemaDeReservas.Domain.Entities
{
    public class Reserva
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public Status Status { get; set; }
    }
}
