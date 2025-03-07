using CadastroClienteService.Domain.Entities;
using CadastroClienteService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroClienteService.Domain.Services
{
    public class ClienteService(IClienteRepository clienteRepository)
    {
        private readonly IClienteRepository _clienteRepository = clienteRepository;

        public void AddCliente(Cliente cliente)
        {
            _clienteRepository.Add(cliente);
        }

        public Cliente GetClientById(int id)
        {
            return _clienteRepository.GetById(id);
        }
    }
}