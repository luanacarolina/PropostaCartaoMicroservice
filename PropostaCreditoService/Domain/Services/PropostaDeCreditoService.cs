using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropostaCreditoService.Domain.Services
{
    public class PropostaDeCreditoService(IPropostaDeCreditoRepository propostaRepository)
    {
        private readonly IPropostaDeCreditoRepository _propostaRepository = propostaRepository;

        public void AddProposta(PropostaDeCredito proposta)
        {
            _propostaRepository.Add(proposta);
        }

    }
}