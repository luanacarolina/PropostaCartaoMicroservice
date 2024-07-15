using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartaoCreditoService.Domain.Entities
{
    public class CartaoDeCredito
    {
        public int ClienteId { get; set; }
        public decimal Limite { get; set; }
        public string Numero { get; set; }
    }
}