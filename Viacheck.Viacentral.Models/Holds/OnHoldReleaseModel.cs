using System;
using System.Collections.Generic;
using System.Text;
using Viacheck.Viacentral.Models.Viacheck;

namespace Viacheck.Viacentral.Models.Holds
{
    public class OnHoldReleaseModel
    {
        public OnHoldChecksModelList CheckList
        {
            get;
            set;
        }

        public string User
        {
            get;
            set;
        }

        public string Operation
        {
            get;
            set;
        }
    }
}
