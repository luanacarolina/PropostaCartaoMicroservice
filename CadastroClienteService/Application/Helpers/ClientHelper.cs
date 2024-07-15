using CadastroClienteService.Application.Dtos;
using CadastroClienteService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CadastroClienteService.Application.Helpers
{
    public static class ClientHelper
    {
        public static ClienteDto ToDto(this Cliente cliente)
        {
            if (cliente == null) return null;

            return new ClienteDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome
            };
        }

        public static Cliente ToEntity(this ClienteDto clienteDto)
        {
            if (clienteDto == null) return null;

            return new Cliente
            {
                Id = clienteDto.Id,
                Nome = clienteDto.Nome
            };
        }
    }
}