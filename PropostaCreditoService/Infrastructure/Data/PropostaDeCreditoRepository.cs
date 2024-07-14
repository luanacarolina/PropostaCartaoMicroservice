using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropostaCreditoService.Infrastructure.Data
{
    public class PropostaDeCreditoRepository : IPropostaDeCreditoRepository
    {
        private readonly List<PropostaDeCredito> _propostas = new List<PropostaDeCredito>();

        public void Add(PropostaDeCredito proposta)
        {
            _propostas.Add(proposta);
        }

        public PropostaDeCredito GetByClienteId(int clienteId)
        {
            return _propostas.Find(p => p.ClienteId == clienteId);
        }
    }
}