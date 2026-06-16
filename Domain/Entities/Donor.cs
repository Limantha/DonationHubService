namespace Domain.Entities
{
    public class Donor
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string EMail { get; set; } = string.Empty;
    }
}
