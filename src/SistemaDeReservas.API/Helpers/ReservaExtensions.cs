using SistemaDeReservas.Application.DTOs;
using SistemaDeReservas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Application.Helpers
{
    public static class ReservaExtensions
    {
        public static ReservaDto ToDto(this Reserva reserva)
        {
            return new ReservaDto
            {
                Id = reserva.Id,
                Data = reserva.Data,
                Hora = reserva.Hora,
                Status = reserva.Status,
                UsuarioId = reserva.UsuarioId
            };
        }
    }
}
