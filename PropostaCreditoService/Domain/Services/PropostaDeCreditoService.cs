using Polly;
using Polly.Retry;
using PropostaCreditoService.Domain.Entities;
using PropostaCreditoService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropostaCreditoService.Domain.Services
{
    public class PropostaDeCreditoService
    {
        private readonly IPropostaDeCreditoRepository _propostaRepository;
        private readonly AsyncRetryPolicy _retryPolicy;

        public PropostaDeCreditoService(IPropostaDeCreditoRepository propostaRepository)
        {
            _propostaRepository = propostaRepository;

            _retryPolicy = Policy
                .Handle<Exception>()
                .RetryAsync(3, onRetry: (exception, retryCount) =>
                {
                    // Log de tentativas de retry
                    Console.WriteLine($"Tentativa {retryCount} de retry devido a: {exception.Message}");
                });
        }
        public async Task AddPropostaAsync(PropostaDeCredito proposta)
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                if (new Random().Next(1, 10) <= 3) // 30% de chance de falha
                {
                    throw new Exception("Simulação de falha na adição da proposta de crédito.");
                }
                _propostaRepository.Add(proposta);
            });
        }

    }
}