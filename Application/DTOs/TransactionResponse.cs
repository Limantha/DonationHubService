namespace Application.DTOs
{
    public class TransactionResponse
    {
        public long TranId { get; set; }

        public string TranCode { get; set; } = string.Empty;

        public int TenantId { get; set; }

        public string CharityName { get; set; } = string.Empty;

        public string DonorFullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public int PaymentMethod { get; set; }

        public string Message { get; set; } = string.Empty;

        public short StatusId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
