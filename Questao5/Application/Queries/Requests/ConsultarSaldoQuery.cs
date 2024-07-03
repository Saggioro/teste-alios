using MediatR;
using Questao5.Application.DTOs;

namespace Questao5.Application.Queries.Requests
{
    public class ConsultarSaldoQuery : IRequest<SaldoDto>
    {
        public string IdContaCorrente { get; set; }
    }
}
