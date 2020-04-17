using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Viacheck.Viacentral.Business;
using Viacheck.Viacentral.Models;
using Viacheck.Viacentral.Models.Agencies;
using Viam.Framework.Logger;

namespace Viacheck.Viacentral.Controllers.Controllers.Agencies
{
    [Route("api/Agency")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AgencyController : Controller
    {
        private AgencyBusiness _agencyBussines;
        private readonly IOptions<ConfigurationModel> _configurations;
        private readonly IViamLogger _viamLogger;

        public AgencyController(IOptions<ConfigurationModel> configurations, IViamLogger log)
        {
            _configurations = configurations;
            _viamLogger = log;
            _agencyBussines = new AgencyBusiness(_configurations.Value);
        }


        [HttpGet]
        [Route("GetAllAgencies")]
        public async Task<List<AgencyModel>> GetAllAgencies()
        {
            try
            {
                return _agencyBussines.GetAllAgencies();

            }
            catch (Exception ex)
            {
                _viamLogger.Error(ex);
                return null;
            }
        }


    }
}