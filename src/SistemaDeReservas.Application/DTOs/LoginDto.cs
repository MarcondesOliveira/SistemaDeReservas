using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Application.DTOs
{
    public class LoginDto
    {
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }
}
