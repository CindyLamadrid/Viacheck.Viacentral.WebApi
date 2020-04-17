using System;
using System.Collections.Generic;
using System.Text;
using Viacheck.Viacentral.Models.Viacheck;

namespace Viacheck.Viacentral.Models.Holds
{
    public class OnHoldCheckUpdateModel
    {
        public OnHoldChecksModel Check
        {
            get;
            set;
        }

        public string User
        {
            get;
            set;
        }
    }
}
