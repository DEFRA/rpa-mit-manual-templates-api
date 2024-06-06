namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// used in the initial invoice header screen
    /// </summary>
    public sealed class DeliveryBodyInitial
    {
        public string AccountCode { get; set; } = string.Empty;
        public string Org { get; set; } = string.Empty;
    }
}
