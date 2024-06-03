
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

            using (var connection = new NpgsqlConnection(DbConn))
            {
                var sql = @"
                        SELECT accountcode, org FROM delivery_body_initial_selections;
                        SELECT name, deliverybodycode FROM schemeinvoicetemplates;
                        SELECT id, name FROM schemeinvoicetemplatessecondaryrpaquestions;
                        SELECT code, description FROM payment_types;
                        SELECT code, description FROM scheme_types;
                        SELECT code, description FROM scheme_codes;
                        SELECT code, description FROM account_codes;
                        SELECT code, description FROM delivery_body_codes;
                        SELECT code, description FROM marketing_year_codes;
                        SELECT code, description FROM fund_codes;
                        ";

                using (var res = await connection.QueryMultipleAsync(sql))
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
            using (var connection = new NpgsqlConnection(DbConn))
            {
                var sql = @"SELECT code, description FROM organisations;";

                return await connection.QueryAsync<Organisation>(sql);
            }
        }

        public async Task<IEnumerable<PaymentType>> GetPaymentTypeReferenceData()
        {
            using (var connection = new NpgsqlConnection(DbConn))
            {
                var sql = @"SELECT code, description FROM payment_types;";

                return await connection.QueryAsync<PaymentType>(sql);
            }
        }

        public async Task<IEnumerable<SchemeType>> GetSchemeTypeReferenceData()
        {
            using (var connection = new NpgsqlConnection(DbConn))
            {
                var sql = @"SELECT code, description FROM scheme_types;";

                return await connection.QueryAsync<SchemeType>(sql);
            }
        }
    }
}
