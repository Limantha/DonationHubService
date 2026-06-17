using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<CreateTransactionResponse> CreateAsync(
            CreateTransactionRequest request,
            CancellationToken cancellationToken = default)
        {
            Validate(request);

            var transaction = new Transaction
            {
                DonorFullName = request.DonorFullName.Trim(),
                Email = request.Email.Trim(),
                Amount = request.Amount,
                PaymentMethod = new ListValue { ListValueId = request.PaymentMethod },
                Message = request.Message.Trim(),
                Status = new ListValue { ListValueId = request.StatusId }
            };

            var transactionId = await _transactionRepository.CreateAsync(transaction, cancellationToken);

            return new CreateTransactionResponse
            {
                TranId = transactionId,
                Message = "Transaction saved successfully."
            };
        }

        private static void Validate(CreateTransactionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.DonorFullName))
            {
                throw new ArgumentException("Donor full name is required.", nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("Email is required.", nameof(request));
            }

            if (request.Amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero.", nameof(request));
            }

            if (request.PaymentMethod <= 0)
            {
                throw new ArgumentException("Payment method is required.", nameof(request));
            }

            if (request.StatusId <= 0)
            {
                throw new ArgumentException("Status is required.", nameof(request));
            }
        }
    }
}
