using System;
using System.Collections.Generic;
using System.Text;
using Viacheck.Viacentral.Models.Holds;

namespace Viacheck.Viacentral.Models.Viacheck
{
    public class OnHoldChecksModel
    {

        public int CheckID { get; set; }
        public int BatchID { get; set; }
        public Nullable<short> CheckStatus { get; set; }
        public Nullable<decimal> Amount { get; set; }       
        public string Transit { get; set; }
        public string CheckNumber { get; set; }      
        public string Account { get; set; }   
        public Nullable<DateTime> Created { get; set; }
        public string LocationName { get; set; }
        public string RefImageKey { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> OwnLicense { get; set; }
        public Nullable<bool> FinCen { get; set; }
        public string NameHold { get; set; }
        public long IdHold { get; set; }
        public string HighlightedField { get; set; }
        public int LocationId { get; set; }
        public int IdGroupHold { get; set; }
        public string GiactValidation { get; set; }
        public int TotalHolds { get; set; }
        public string Description { get; set; }
        public string HoldColor { get; set; }
        public bool ShowDescription { get; set; }
        public Int16 HoldPriority { get; set; }
        public string NamePriority { get; set; }
        public string OriginalTransit { get; set; }
        public Nullable<decimal> OriginalAmount { get; set; }
        public string OriginalCheckNumber { get; set; }
        public string OriginalAccount { get; set; }
        public string DocumentList { get; set; }
        public string HoldDescription { get; set; }
        public string TotalAllHolds { get; set; }
        public string IsDefinitive { get; set; }
        public bool VerifyHoldNoShowed { get; set; }
        public List<OnHoldDescriptionModel> Holds { get; set; }
        public bool IsValidAmount { get; set; }
        public bool IsValidCheckNumber { get; set; }
        public bool IsValidAccount { get; set; }
        public bool IsValidRouting { get; set; }

        public OnHoldChecksModel()
        {
            IsValidAmount = true;
            IsValidCheckNumber = true;
            IsValidAccount = true;
            IsValidRouting = true;
        }

    }
}
