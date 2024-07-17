using SistemaDeReservas.Domain.Enum;
using SistemaDeReservas.Domain.Inputs;

namespace SistemaDeReservas.Domain.Entities
{
    public class Reserva : Entity
    {
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public Status Status { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public Reserva()
        {

        }

        public Reserva(CreateReservaInput input)
        {
            Data = input.Data;
            Hora = TimeSpan.Parse(input.Hora);
            Status = Status.Pendente;
            UsuarioId = input.UsuarioId;
        }

        public Reserva(UpdateReservaInput input)
        {
            Id = input.Id;
            Data = input.Data;
            Hora = TimeSpan.Parse(input.Hora);
            Status = input.Status;
            UsuarioId = input.UsuarioId;
        }
    }
}
