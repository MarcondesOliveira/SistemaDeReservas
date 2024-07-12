using SistemaDeReservas.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Application.DTOs
{
    public class ReservaDto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public Status Status { get; set; }
        public int UsuarioId { get; set; }
    }
}
