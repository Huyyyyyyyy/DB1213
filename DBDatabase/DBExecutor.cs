using System.Data;
using Npgsql;
using DBUtils;

namespace DBDatabase
{
    public class DBExecutor : IAsyncDisposable
    {
        private readonly string _connectionString;
        private readonly SimpleLogger _logger;

        public DBExecutor(string connectionString, SimpleLogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, Dictionary<string, object?>? parameters = null, CancellationToken token = default)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(token);
            await using var cmd = new NpgsqlCommand(sql, conn);
            AddParameters(cmd, parameters);

            var rows = await cmd.ExecuteNonQueryAsync(token);
            await conn.DisposeAsync();
            return rows;
        }

        public async Task<T?> ExecuteSingleQueryAsync<T>(
            string sql,
            Func<IDataReader, T> map,
            Dictionary<string, object?>? parameters = null,
            CancellationToken token = default)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(token);
            await using var cmd = new NpgsqlCommand(sql, conn);
            AddParameters(cmd, parameters);
            await using var reader = await cmd.ExecuteReaderAsync(token);
            var rs = default(T?);
            if (await reader.ReadAsync(token)) rs = map(reader);
            await conn.DisposeAsync();
            return rs;
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string sql, Func<IDataReader, T> map, Dictionary<string, object?>? parameters = null, CancellationToken token = default)
        {
            var list = new List<T>();
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(token);
            await using var cmd = new NpgsqlCommand(sql, conn);
            AddParameters(cmd, parameters);
            await using var reader = await cmd.ExecuteReaderAsync(token);

            while (await reader.ReadAsync(token))
                list.Add(map(reader));
            await conn.DisposeAsync();
            return list;
        }

        public async Task ExecuteTransactionAsync(Func<NpgsqlTransaction, Task> action, CancellationToken token = default)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(token);
            await using var tx = await conn.BeginTransactionAsync(token);
            try
            {
                await action(tx);
                await tx.CommitAsync(token);
                await conn.DisposeAsync();
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(token);
                await conn.DisposeAsync();
                _logger.Debug($"Executing SQL: {"Transaction rolled back"} with details : {ex}");
                throw;
            }
        }

        private static void AddParameters(NpgsqlCommand cmd, Dictionary<string, object?>? parameters)
        {
            if (parameters == null) return;
            foreach (var (key, value) in parameters)
                cmd.Parameters.AddWithValue(key, value ?? DBNull.Value);
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
