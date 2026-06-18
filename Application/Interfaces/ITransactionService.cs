using Application.DTOs;

namespace Application.Interfaces
{
    public interface ITransactionService
    {
        Task<CreateTransactionResponse> CreateAsync(
            CreateTransactionRequest request,
            CancellationToken cancellationToken = default);

        Task<PagedResult<TransactionResponse>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
