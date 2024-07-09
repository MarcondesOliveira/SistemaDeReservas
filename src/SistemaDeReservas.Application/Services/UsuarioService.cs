using SistemaDeReservas.Application.DTOs;
using SistemaDeReservas.Domain.Entities;
using SistemaDeReservas.Domain.Inputs;
using SistemaDeReservas.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeReservas.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Usuario>> GetAll()
        {
            return await _repository.GetAll();
        }

        public void Create(CreateUsuarioInput input)
        {
            var usuario = new Usuario(input);

            _repository.Create(usuario);
        }

        public Usuario ObterPorId(int id)
        {
            return _repository.GetById(id);
        }

        public void Update(UpdateUsuarioInput input)
        {
            var usuario = new Usuario(input);

            _repository.Update(usuario);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
