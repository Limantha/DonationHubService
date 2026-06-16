using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<long> CreateAsync(Transaction transaction, CancellationToken cancellationToken = default);
    }
}
