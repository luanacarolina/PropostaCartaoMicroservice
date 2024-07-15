using PropostaCreditoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropostaCreditoService.Domain.Interfaces
{
    public interface IPropostaDeCreditoRepository
    {
        void Add(PropostaDeCredito proposta);
        PropostaDeCredito GetByClienteId(int clienteId);
    }
}