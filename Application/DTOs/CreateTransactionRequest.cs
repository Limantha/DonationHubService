namespace Application.DTOs
{
    public class CreateTransactionRequest
    {
        public string TranCode { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public int PaymentMethod { get; set; }

        public string Message { get; set; } = string.Empty;

        public long DonorId { get; set; }

        public int StatusId { get; set; }
    }
}
