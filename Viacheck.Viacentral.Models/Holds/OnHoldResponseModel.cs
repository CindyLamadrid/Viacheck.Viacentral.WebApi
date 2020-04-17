using System;
using System.Collections.Generic;
using System.Text;
using Viacheck.Viacentral.Models.Viacheck;

namespace Viacheck.Viacentral.Models.Holds
{
    public class OnHoldResponseModel
    {
        public MessageResponseModel Status
        {
            get;
            set;
        }

        public List<OnHoldChecksModel> HoldResults
        {
            get;
            set;
        }

        public AccountInformationModel AccountInformation
        {
            get;
            set;
        }

        public List<CategorySummaryModel> CategorySummary
        {
            get;
            set;
        }

        public List<OnHoldLegendModel> OnHoldLegend
        {
            get;
            set;
        }

        public List<AgencySummaryModel> AgencySummary
        {
            get;
            set;
        }
    }
}
