namespace Domain.Entities
{
    public class ListValue
    {
        public short ListId { get; set; }

        public string ListName { get; set; } = string.Empty;

        public short ListValueId { get; set; }

        public string ListValueName { get; set; } = string.Empty;

        public string Remarks { get; set; } = string.Empty;
    }
}
