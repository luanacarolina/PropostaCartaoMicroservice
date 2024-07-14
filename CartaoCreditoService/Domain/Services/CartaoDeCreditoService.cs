using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartaoCreditoService.Domain.Services
{
    public class CartaoDeCreditoService(ICartaoDeCreditoRepository cartaoRepository)
    {
        private readonly ICartaoDeCreditoRepository _cartaoRepository = cartaoRepository;

        public void AddCartao(CartaoDeCredito cartao)
        {
            _cartaoRepository.Add(cartao);
        }

    }
}