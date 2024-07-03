using MediatR;
using Questao5.Application.DTOs;
using Questao5.Application.Queries.Requests;
using Questao5.Infrastructure.Services;

namespace Questao5.Application.Handlers
{
    public class ConsultarSaldoHandler : IRequestHandler<ConsultarSaldoQuery, SaldoDto>
    {
        private readonly IContaCorrenteService _contaCorrenteService;

        public ConsultarSaldoHandler(IContaCorrenteService contaCorrenteService)
        {
            _contaCorrenteService = contaCorrenteService;
        }

        public async Task<SaldoDto> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
        {
            return await _contaCorrenteService.ConsultarSaldoAsync(request.IdContaCorrente);
        }
    }
}
