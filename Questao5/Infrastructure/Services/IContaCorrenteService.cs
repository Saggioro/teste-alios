using Questao5.Application.Commands.Requests;
using Questao5.Application.DTOs;
using Questao5.Application.Queries.Requests;

namespace Questao5.Infrastructure.Services
{
    public interface IContaCorrenteService
    {
        Task<string> MovimentarAsync(MovimentarContaCorrenteCommand command);
        Task<SaldoDto> ConsultarSaldoAsync(string idContaCorrente);
    }
}