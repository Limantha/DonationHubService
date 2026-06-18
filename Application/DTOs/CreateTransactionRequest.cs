namespace Application.DTOs
{
    public class CreateTransactionRequest
    {
        public int TenantId { get; set; }

        public string DonorFullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public int PaymentMethod { get; set; }

        public string Message { get; set; } = string.Empty;

        public int StatusId { get; set; }
    }
}
