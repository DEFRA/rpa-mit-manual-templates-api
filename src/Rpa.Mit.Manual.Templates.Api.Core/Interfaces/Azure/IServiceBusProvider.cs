﻿namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure
{
    public interface IServiceBusProvider
    {
        /// <summary>
        /// send invoice request and assocoated invoice lines as json to payment hub
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendInvoiceRequestJson(string msg);
    }
}
