namespace Domain
{
    public class Transaction
    {
        public long TranId { get; set; }

        public string TranCode { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public long DonorId { get; set; }

        public short StatusId { get; set; }
    }
}
