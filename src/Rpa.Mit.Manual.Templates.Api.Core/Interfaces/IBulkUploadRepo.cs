﻿using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IBulkUploadRepo
    {
        Task<bool> AddApBulkUpload(BulkUploadApDataset bulkUploadApDataset, CancellationToken ct);

        /// <summary>
        /// gets the concatened detail line description when we do a bulk upload
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<string> GetDetailLineDescripion(string query, CancellationToken ct);

        /// <summary>
        /// confirm or reject a bulk upload
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns>true if confirn, false if reject</returns>
        Task<bool> ConfirmOrReject(BulkUploadConfirmation request, CancellationToken ct);
    }
}
