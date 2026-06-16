using Application.DTOs;

namespace Application.Interfaces
{
    public interface ITransactionService
    {
        Task<CreateTransactionResponse> CreateAsync(
            CreateTransactionRequest request,
            CancellationToken cancellationToken = default);
    }
}
