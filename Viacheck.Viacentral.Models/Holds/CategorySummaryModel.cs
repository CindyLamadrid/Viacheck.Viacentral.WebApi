using System;
using System.Collections.Generic;
using System.Text;

namespace Viacheck.Viacentral.Models.Holds
{
    public class CategorySummaryModel
    {
        public int IdCategory { get; set; }
        public string HoldGroup { get; set; }
        public int Total { get; set; }
        public bool DefaultCategory { get; set; }
    }
}
