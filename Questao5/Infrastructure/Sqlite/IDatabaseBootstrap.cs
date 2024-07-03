using Microsoft.Data.Sqlite;

namespace Questao5.Infrastructure.Sqlite
{
    public interface IDatabaseBootstrap
    {
        void Setup();

        SqliteConnection GetConnection();
    }
}