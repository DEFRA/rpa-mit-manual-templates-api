namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// from the lookup_accounts_ap table
    /// </summary>
    public sealed record AccountAp
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Org { get; set; } = string.Empty;
    }
}
