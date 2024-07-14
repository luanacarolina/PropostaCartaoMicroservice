using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartaoCreditoService.Domain.Interfaces
{
    public interface ICartaoDeCreditoRepository
    {
        void Add(CartaoDeCredito cartao);
        CartaoDeCredito GetByClienteId(int clienteId);
    }
}