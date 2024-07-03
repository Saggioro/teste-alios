using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Infrastructure.Services;
using System;
using System.Threading.Tasks;

namespace Questao5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ContaCorrenteController> _logger;

        public ContaCorrenteController(IMediator mediator, ILogger<ContaCorrenteController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("movimentar")]
        public async Task<IActionResult> Movimentar([FromBody] MovimentarContaCorrenteCommand command)
        {
            try
            {
                var idMovimento = await _mediator.Send(command);
                return Ok(new { IdMovimento = idMovimento });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Mensagem = ex.ErrorType.ToString() });
            }
        }

        [HttpGet("saldo/{idContaCorrente}")]
        public async Task<IActionResult> ConsultarSaldo(string idContaCorrente)
        {
            try
            {
                _logger.LogInformation("Consultando saldo para a conta: {idContaCorrente}", idContaCorrente);
                var saldo = await _mediator.Send(new ConsultarSaldoQuery { IdContaCorrente = idContaCorrente });
                _logger.LogInformation("Saldo consultado: {@Saldo}", saldo);
                return Ok(saldo);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation exception occurred.");
                return BadRequest(new { Mensagem = ex.ErrorType.ToString() });
            }
        }
    }
}