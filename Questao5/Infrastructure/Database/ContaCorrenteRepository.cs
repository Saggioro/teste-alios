using Dapper;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Questao5.Infrastructure.Database
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly IDatabaseBootstrap _databaseBootstrap;

        public ContaCorrenteRepository(IDatabaseBootstrap databaseBootstrap)
        {
            _databaseBootstrap = databaseBootstrap;
        }

        public async Task<ContaCorrente> GetByIdAsync(string id)
        {
            using var connection = _databaseBootstrap.GetConnection();
            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Movimento>> GetMovimentosByContaIdAsync(string id)
        {
            using var connection = _databaseBootstrap.GetConnection();
            return await connection.QueryAsync<Movimento>(
                "SELECT * FROM movimento WHERE idcontacorrente = @Id", new { Id = id });
        }

        public async Task AddMovimentoAsync(Movimento movimento)
        {
            using var connection = _databaseBootstrap.GetConnection();
            await connection.ExecuteAsync(
                "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                "VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
                movimento);
        }
    }
}