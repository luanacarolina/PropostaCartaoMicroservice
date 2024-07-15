using CartaoCreditoService.Domain.Entities;
using CartaoCreditoService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartaoCreditoService.Infrastructure.Data
{
    public class CartaoDeCreditoRepository : ICartaoDeCreditoRepository
    {
        private readonly List<CartaoDeCredito> _cartoes = new List<CartaoDeCredito>();

        public void Add(CartaoDeCredito cartao)
        {
            _cartoes.Add(cartao);
        }

        public CartaoDeCredito GetByClienteId(int clienteId)
        {
            return _cartoes.Find(c => c.ClienteId == clienteId);
        }
    }
}