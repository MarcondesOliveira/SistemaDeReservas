using SistemaDeReservas.Domain.Enum;

namespace SistemaDeReservas.Domain.Entities
{
    public class Notificacao : Entity
    {
        public string Mensagem { get; set; }
        public Tipo Tipo { get; set; }
        public string Destinatario { get; set; }
    }
}
