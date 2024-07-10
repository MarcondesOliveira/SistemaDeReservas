using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Domain.Inputs
{
    public class CreateReservaInput
    {
        public DateTime Data { get; set; }
        public string Hora { get; set; }
        public Status Status { get; set; }
        public int UsuarioId { get; set; }
        //public virtual Usuario Usuario { get; set; }
    }
}