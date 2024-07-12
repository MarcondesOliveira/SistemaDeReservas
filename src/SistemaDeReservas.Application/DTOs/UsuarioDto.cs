using SistemaDeReservas.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Application.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public TipoPermissao Permissao { get; set; }
    }
}
