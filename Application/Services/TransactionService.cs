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

        public async Task<PagedResult<TransactionResponse>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            ValidatePagination(pageNumber, pageSize);

            var (transactions, totalCount) = await _transactionRepository.GetPagedAsync(
                pageNumber,
                pageSize,
                cancellationToken);

            return new PagedResult<TransactionResponse>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = transactions.Select(transaction => new TransactionResponse
                {
                    TranId = transaction.TranId,
                    TranCode = transaction.TranCode,
                    DonorFullName = transaction.DonorFullName,
                    Email = transaction.Email,
                    Amount = transaction.Amount,
                    PaymentMethod = transaction.PaymentMethod.ListValueId,
                    Message = transaction.Message,
                    StatusId = transaction.Status.ListValueId
                }).ToList()
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

        private static void ValidatePagination(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero.", nameof(pageNumber));
            }

            if (pageSize <= 0)
            {
                throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
            }

            if (pageSize > 100)
            {
                throw new ArgumentException("Page size cannot be greater than 100.", nameof(pageSize));
            }
        }
    }
}
