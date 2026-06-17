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
            const string sql = """
                INSERT INTO [dbo].[Transaction]
                    (DonorFullName, Email, Amount, PaymentMethod, Message, StatusId)
                OUTPUT INSERTED.TranId
                VALUES
                    (@DonorFullName, @Email, @Amount, @PaymentMethod, @Message, @StatusId);
                """;

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = sql;

            AddParameter(command, "@DonorFullName", DbType.String, transaction.DonorFullName);
            AddParameter(command, "@Email", DbType.String, transaction.Email);
            AddDecimalParameter(command, "@Amount", transaction.Amount);
            AddParameter(command, "@PaymentMethod", DbType.Int32, transaction.PaymentMethod.ListValueId);
            AddParameter(command, "@Message", DbType.String, transaction.Message);
            AddParameter(command, "@StatusId", DbType.Int32, transaction.Status.ListValueId);

            var result = await command.ExecuteScalarAsync(cancellationToken);
            transaction.TranId = Convert.ToInt64(result);

            return transaction.TranId;
        }

        private static void AddParameter(DbCommand command, string name, DbType dbType, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = dbType;
            parameter.Value = value;
            command.Parameters.Add(parameter);
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
