namespace Domain.Entities
{
    public class Tenant
    {
        public int TenantId { get; set; }

        public string TenantName { get; set; } = string.Empty;

        public string GatewayType { get; set; } = string.Empty;

        public string ApiKey { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
