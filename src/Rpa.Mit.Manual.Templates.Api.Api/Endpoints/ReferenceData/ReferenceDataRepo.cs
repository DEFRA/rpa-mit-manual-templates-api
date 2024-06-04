
using System.Data;

using Dapper;

using Microsoft.Extensions.Options;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Api.Endpoints;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.ReferenceDataEndPoint
{
    public class ReferenceDataRepo : BaseData, IReferenceDataRepo
    {
        public ReferenceDataRepo(IOptions<ConnectionStrings> options) : base(options)
        { } 

        public async Task<ReferenceData> GetAllReferenceData()
        {
            var referenceData = new ReferenceData();

            using (var cn = new NpgsqlConnection(DbConn))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync();

                var sql = @"
                        SELECT accountcode, org FROM lookup_deliverybodyinitialselections;
                        SELECT name, deliverybodycode FROM lookup_schemeinvoicetemplates;
                        SELECT id, name FROM lookup_schemeinvoicetemplatessecondaryrpaquestions;
                        SELECT code, description FROM lookup_paymenttypes;
                        SELECT code, description FROM lookup_schemetypes;
                        SELECT code, description FROM lookup_schemecodes;
                        SELECT code, description FROM lookup_accountcodes;
                        SELECT code, description FROM lookup_deliverybodycodes;
                        SELECT code, description FROM lookup_marketingyearcodes;
                        SELECT code, description FROM lookup_fundcodes;
                        ";

                using (var res = await cn.QueryMultipleAsync(sql))
                {
                    referenceData.InitialDeliveryBodies = await res.ReadAsync<DeliveryBodyInitial>();
                    referenceData.SchemeInvoiceTemplates = await res.ReadAsync<SchemeInvoiceTemplate>();
                    referenceData.SchemeInvoiceTemplateSecondaryQuestions = await res.ReadAsync<SchemeInvoiceTemplateSecondaryQuestion>();
                    referenceData.PaymentTypes = await res.ReadAsync<PaymentType>();
                    referenceData.SchemeTypes = await res.ReadAsync<SchemeType>();
                    referenceData.SchemeCodes = await res.ReadAsync<SchemeCode>();
                    referenceData.AccountCodes = await res.ReadAsync<AccountCode>();
                    referenceData.DeliveryBodies = await res.ReadAsync<DeliveryBody>();
                    referenceData.MarketingYears = await res.ReadAsync<MarketingYear>();
                    referenceData.FundCodes = await res.ReadAsync<FundCode>();

                    return referenceData;
                }
            }
        }

        public async Task<IEnumerable<Organisation>> GetOrganisationsReferenceData()
        {
            using (var cn = new NpgsqlConnection(DbConn))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync();

                var sql = @"SELECT code, description FROM organisations;";

                return await cn.QueryAsync<Organisation>(sql);
            }
        }

        public async Task<IEnumerable<PaymentType>> GetPaymentTypeReferenceData()
        {
            using (var cn = new NpgsqlConnection(DbConn))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync();

                var sql = @"SELECT code, description FROM lookup_paymenttypes;";

                return await cn.QueryAsync<PaymentType>(sql);
            }
        }

        public async Task<IEnumerable<SchemeType>> GetSchemeTypeReferenceData()
        {
            using (var cn = new NpgsqlConnection(DbConn))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync();

                var sql = @"SELECT code, description FROM lookup_schemetypes;";

                return await cn.QueryAsync<SchemeType>(sql);
            }
        }
    }
}
