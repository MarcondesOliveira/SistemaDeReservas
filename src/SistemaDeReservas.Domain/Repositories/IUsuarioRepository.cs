﻿using SistemaDeReservas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Domain.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario ObterPorNomeUsuarioESenha(string email, string senha);
    }
}
