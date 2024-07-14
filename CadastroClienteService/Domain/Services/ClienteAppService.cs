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
            var cliente = clienteDto.ToEntity();
            _clienteService.AddCliente(cliente);
        }

        public ClienteDto GetCliente(int id)
        {
            var cliente = _clienteService.GetClienteById(id);
            return cliente.ToDto();
        }

    }
}