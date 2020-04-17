using System;
using System.Collections.Generic;
using System.Text;

namespace Viacheck.Viacentral.Models.Holds
{
    public class OnHoldFiltersModels
    {
        public int? AgencyCode { get; set; }
        public int? CategoryHold { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal? Amount { get; set; }
        public string User { get; set; }
        public int PageSize { get; set; }
        public int PageNum { get; set; }
        public string IdHoldLegend { get; set; }
    }
}
