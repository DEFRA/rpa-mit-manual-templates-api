﻿using Npgsql;
using System.Data;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Microsoft.Extensions.Options;
using Dapper;
using System.Diagnostics.CodeAnalysis;
using Invoices.Add;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceRepo : BaseData, IInvoiceRepo
    {
        public InvoiceRepo(IOptions<ConnectionStrings> options) : base(options)
        { }

        public async Task<bool> AddInvoice(Invoice invoice, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(DbConn))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = @"INSERT INTO Invoices (Id, SchemeType, Reference, Value, Status, ApproverId, ApproverEmail, ApprovedBy, Approved, CreatedBy, Created, PaymentType, AccountType, DeliveryBody)
                                VALUES (@Id, @SchemeType, @Reference, @Value, @Status, @ApproverId, @ApproverEmail, @ApprovedBy, @Approved, @CreatedBy, @Created, @PaymentType, @AccountType, @DeliveryBody )";

                var res = await cn.ExecuteAsync(sql, invoice);

                return res == 1;
            }
        }
    }
}
