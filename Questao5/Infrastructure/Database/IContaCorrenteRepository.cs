using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database
{
    public interface IContaCorrenteRepository
    {
        Task<ContaCorrente> GetByIdAsync(string id);
        Task<IEnumerable<Movimento>> GetMovimentosByContaIdAsync(string id);
        Task AddMovimentoAsync(Movimento movimento);
    }
}
