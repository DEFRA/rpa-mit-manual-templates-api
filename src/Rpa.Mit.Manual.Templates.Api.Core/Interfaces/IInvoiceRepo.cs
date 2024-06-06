using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IInvoiceRepo
    {
        Task<bool> AddInvoice(Invoice invoice);
    }
}
