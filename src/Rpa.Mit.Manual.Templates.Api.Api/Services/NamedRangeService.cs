using System;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Services
{
    public class NamedRangeService : INamedRangeService
    {
        public string GetFundCodeNamedRange(string org, string dBody, string invoiceType)
        {
            string fund;

            if (org == "RPA")
            {
                switch (dBody)
                {
                    case "XG,INT":
                        fund = "EXQFund";
                        break;
                    case "IP":
                        fund = "RPAIPFunds";
                        break;
                    case "OPA":
                        fund = "OPAFunds";
                        break;
                    case "HE":
                        fund = "HEFunds";
                        break;
                    default:
                        fund = org + "Funds";
                        break;
                }
            }
            else
            {
                fund = GetFundCodeForNonRPA(dBody, org, invoiceType);
            }

            return fund;
        }

        public string GetDeliveryBodyNamedRange(string org, string dBody)
        {
            throw new NotImplementedException();
        }

        public string GetAccountNamedRange(string org, string dBody, string invoiceType)
        {
            throw new NotImplementedException();
        }

        public string GetMarketingYearNamedRange(string org, string dBody)
        {
            string marketingYear = "NSMY";

            switch (dBody.Right(2))
            {
                case "P1":
                    marketingYear = "P1MY";
                    break;
                case "XQ":
                    marketingYear = "ExNRDPEMY";
                    break;
            }

            if (dBody.Right(2) == "LS" || dBody.Right(5) == "LSDom")
            {
                marketingYear = "LSMY";
            }
            else if (dBody.Right(2) == "CS" || dBody.Right(5) == "CSDom")
            {
                marketingYear = "NAMY"; // a-VA02-02 request from NE to allow NA for EXQ lines (applies to FC as well)
            }

            switch (dBody)
            {
                case "SPS,TR":
                    marketingYear = "LSMY";
                    break;
                case "XG":
                    marketingYear = "XGMY";
                    break;
                case "OPA":
                    marketingYear = "OPAMY";
                    break;
            }

            return marketingYear;
        }

        public string GetSchemeTypeNamedRange(string org, string dBody)
        {
            throw new NotImplementedException();
        }

        private string GetFundCodeForNonRPA(string dBody, string org, string invoiceType)
        {
            string fundCode = "";

            if (dBody.Right(2) == "P1")
            {
                // What funds for NE, FC, RDT, RDPE schemes - LS or NS
                fundCode = "P1Funds";
            }
            else if (dBody.Right(2) == "XQ")
            {
                fundCode = "ExNRDPEFunds";
            }
            else if (dBody.Right(2) == "LS" && invoiceType == "AR")
            {
                if (org == "NE" || org == "FC" || org == "RDPE" || org == "RDT")
                {
                    fundCode = "AR_LS_FUNDS";
                }
                else
                {
                    fundCode = "LSFunds";
                }
            }
            else if (dBody.Right(3) == "Dom" && invoiceType == "AR")
            {
                if (org == "NE" || org == "FC" || org == "RDPE" || org == "RDT")
                {
                    fundCode = "AR_DOM_FUNDS";
                }
            }
            else if (org == "FC" && invoiceType == "AP") //    'v02-05 added to provide EXQ99 for FC AP (pv)
            {
                fundCode = "FCAP";
            }
            else if (dBody.Right(2) == "EA")            // a-VA03-00 change to accomodate EA Invoices
            {
                if (dBody.Right(5) == "CSDom" || invoiceType == "AP")
                {
                    fundCode = "EA_DOM_FUNDS";
                }
                else
                {
                    fundCode = "EAFunds";
                }
            }
            else
            {
                if (org == "RDT" && invoiceType == "AP")
                {
                    fundCode = "RDTNSFunds";
                }
                else if (dBody == "NECS" && invoiceType == "AP")
                {
                    fundCode = "NECSFunds";
                }
                else if (invoiceType == "AR")
                {
                    fundCode = "NSARFunds";
                }
                else
                {
                    fundCode = "NSFunds";
                }
            }

            return fundCode;
        }
    }
}
