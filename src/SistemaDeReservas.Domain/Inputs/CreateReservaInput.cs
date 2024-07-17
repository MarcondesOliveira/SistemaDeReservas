using SistemaDeReservas.Domain.Enum;

namespace SistemaDeReservas.Domain.Inputs
{
    public class CreateReservaInput
    {
        //public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Hora { get; set; }
        public Status Status { get; set; }
        public int UsuarioId { get; set; }
    }
}