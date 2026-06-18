using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<long> CreateAsync(Transaction transaction, CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<Transaction> Transactions, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
