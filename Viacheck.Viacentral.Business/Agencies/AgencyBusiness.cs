using System;
using System.Collections.Generic;
using System.Text;
using Viacheck.Viacentral.Agencies;
using Viacheck.Viacentral.Models;
using Viacheck.Viacentral.Models.Agencies;

namespace Viacheck.Viacentral.Business
{
    public class AgencyBusiness
    {
        private AgencyRepository _agencyRepositoryRead;

        public AgencyBusiness(ConfigurationModel configuration)
        {
            _agencyRepositoryRead = new AgencyRepository(configuration.ViacheckRead);
        }

        /// <summary>
        /// Get Agency list
        /// </summary>
        /// <returns></returns>
        public List<AgencyModel> GetAllAgencies()
        {
            return _agencyRepositoryRead.GetAllAgencies();
        }

    }
}
