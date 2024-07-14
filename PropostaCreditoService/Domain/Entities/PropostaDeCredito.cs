using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropostaCreditoService.Domain.Entities
{
    public class PropostaDeCredito
    {
        public int ClienteId { get; set; }
        public decimal Limite { get; set; }
    }
}