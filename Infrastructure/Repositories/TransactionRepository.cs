using System.Data;
using System.Data.Common;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TransactionRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<long> CreateAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = "dbo.CreateTransaction";
            command.CommandType = CommandType.StoredProcedure;

            AddParameter(command, "@DonorFullName", DbType.String, transaction.DonorFullName);
            AddParameter(command, "@Email", DbType.String, transaction.Email);
            AddDecimalParameter(command, "@Amount", transaction.Amount);
            AddParameter(command, "@PaymentMethod", DbType.Int32, transaction.PaymentMethod.ListValueId);
            AddParameter(command, "@Message", DbType.AnsiString, transaction.Message);
            AddParameter(command, "@StatusId", DbType.Int32, transaction.Status.ListValueId);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (!await reader.ReadAsync(cancellationToken))
            {
                throw new InvalidOperationException("CreateTransaction did not return a transaction result.");
            }

            transaction.TranId = reader.GetInt64(reader.GetOrdinal("TranId"));
            transaction.TranCode = reader.GetString(reader.GetOrdinal("TranCode"));

            return transaction.TranId;
        }

        public async Task<(IReadOnlyList<Transaction> Transactions, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = "dbo.GetTransactionsPaged";
            command.CommandType = CommandType.StoredProcedure;

            AddParameter(command, "@PageNumber", DbType.Int32, pageNumber);
            AddParameter(command, "@PageSize", DbType.Int32, pageSize);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var totalCount = 0;
            if (await reader.ReadAsync(cancellationToken))
            {
                totalCount = reader.GetInt32(0);
            }

            var transactions = new List<Transaction>();
            if (await reader.NextResultAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    transactions.Add(new Transaction
                    {
                        TranId = GetInt64(reader, "TranId"),
                        TranCode = GetString(reader, "TranCode"),
                        DonorFullName = GetString(reader, "DonorFullName"),
                        Email = GetString(reader, "Email"),
                        Amount = GetDecimal(reader, "Amount"),
                        PaymentMethod = new ListValue { ListValueId = GetInt32(reader, "PaymentMethod") },
                        Message = GetString(reader, "Message"),
                        Status = new ListValue { ListValueId = GetInt32(reader, "StatusId") }
                    });
                }
            }

            return (transactions, totalCount);
        }

        private static void AddParameter(DbCommand command, string name, DbType dbType, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = dbType;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }

        private static string GetString(DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
        }

        private static int GetInt32(DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
        }

        private static long GetInt64(DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? 0 : reader.GetInt64(ordinal);
        }

        private static decimal GetDecimal(DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? 0 : reader.GetDecimal(ordinal);
        }

        private static void AddDecimalParameter(DbCommand command, string name, decimal value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = DbType.Decimal;
            parameter.Precision = 18;
            parameter.Scale = 2;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
    }
}
