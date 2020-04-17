using System;
using System.Collections.Generic;
using System.Text;

namespace Viacheck.Viacentral.Models
{
    public class ConfigurationModel
    {
        public string ViacheckWriter
        {
            get;
            set;
        }

        public string ViacheckRead
        {
            get;
            set;
        }

        public string Version
        {
            get;
            set;
        }

        public string HostFromAllowCORS
        {
            get;
            set;
        }
    }
}
