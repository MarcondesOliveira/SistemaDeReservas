﻿namespace SistemaDeReservas.Domain.Inputs
{
    public class UsuarioInput
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }
}
