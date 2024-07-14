using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CadastroClienteService.Domain.Entities;

namespace CadastroClienteService.Infrastructure.Data
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly List<Cliente> _clientes = new List<Cliente>();

        public void Add(Cliente cliente)
        {
            _clientes.Add(cliente);
        }

        public Cliente GetById(int id)
        {
            return _clientes.Find(c => c.Id == id);
        }
    }
}