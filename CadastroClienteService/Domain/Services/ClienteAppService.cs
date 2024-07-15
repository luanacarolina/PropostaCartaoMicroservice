using CadastroClienteService.Application.Dtos;
using CadastroClienteService.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroClienteService.Domain.Services
{
    public class ClienteAppService(ClienteService clienteService)
    {
        private readonly ClienteService _clienteService = clienteService;

        public void AddCliente(ClienteDto clienteDto)
        {
            var cliente = ClientHelper.ToEntity(clienteDto);
            _clienteService.AddCliente(cliente);
        }

        public ClienteDto GetCliente(int id)
        {
            var cliente = _clienteService.GetClientById(id);
            return cliente.ToDto();
        }

    }
}