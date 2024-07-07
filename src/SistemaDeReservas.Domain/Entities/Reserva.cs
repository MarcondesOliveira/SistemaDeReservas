using SistemaDeReservas.Domain.Enum;

namespace SistemaDeReservas.Domain.Entities
{
    public class Reserva : Entity
    {
        public DateTime Data { get; set; }
        public Status Status { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
