using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroClienteService.Domain.Interfaces
{
    public interface IClienteRepository
    {
        void Add(Cliente cliente);
        Cliente GetById(int id);
    }
}