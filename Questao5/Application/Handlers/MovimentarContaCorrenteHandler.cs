using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Infrastructure.Services;

namespace Questao5.Application.Handlers
{
    public class MovimentarContaHandler : IRequestHandler<MovimentarContaCorrenteCommand, string>
    {
        private readonly IContaCorrenteService _contaCorrenteService;

        public MovimentarContaHandler(IContaCorrenteService contaCorrenteService)
        {
            _contaCorrenteService = contaCorrenteService;
        }

        public async Task<string> Handle(MovimentarContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            return await _contaCorrenteService.MovimentarAsync(request);
        }
    }
}
