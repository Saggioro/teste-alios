using MediatR;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentarContaCorrenteCommand : IRequest<string>
    {
        public string Id { get; set; }
        public decimal Valor { get; set; }
        public TipoMovimento TipoMovimento { get; set; }
        public string ChaveIdempotencia { get; set; }
    }
}
