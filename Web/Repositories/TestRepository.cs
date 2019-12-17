using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Maximiz.Repositories
{
    public class TestRepository
    {
        private readonly string connectionString;

        public TestRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DatabaseInternal");
        }

        public async Task<bool> DoThingInScope()
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    await GetLocks();
                    await One();
                    await Two();
                    transactionScope.Complete();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        private async Task One()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                var sql = $"UPDATE debug.users " +
                    $"SET name = 'yeet' " +
                    $"WHERE id=1";
                await connection.ExecuteAsync(sql);
            }
        }

        private async Task Two()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                var sql = $"INSERT INTO debug.users (id, name) " +
                    $"VALUES (3, 'namethree');";
                await connection.ExecuteAsync(sql);
            }
        }

        private async Task GetLocks()
        {

        }

    }
}
