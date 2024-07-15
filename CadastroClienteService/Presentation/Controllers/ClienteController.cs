using CadastroClienteService.Application.Dtos;
using CadastroClienteService.Domain.Services;
using CadastroClienteService.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroClienteService.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteAppService _clienteAppService;
        private readonly RabbitMQClientService _rabbitMQClientService;

        public ClienteController(ClienteAppService clienteAppService, RabbitMQClientService rabbitMQClientService)
        {
            _clienteAppService = clienteAppService;
            _rabbitMQClientService = rabbitMQClientService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ClienteDto clienteDto)
        {
            _clienteAppService.AddCliente(clienteDto);
            _rabbitMQClientService.SendMessage(clienteDto);
            return Ok();
        }
    }
}