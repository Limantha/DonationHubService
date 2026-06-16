namespace Domain.Entities
{
    public class Transaction
    {
        public long TranId { get; set; }

        public string TranCode { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public ListValue PaymentMethod { get; set; } = new();

        public string Message { get; set; } = string.Empty;

        public long DonorId { get; set; }

        public ListValue Status { get; set; } = new();
    }
}
