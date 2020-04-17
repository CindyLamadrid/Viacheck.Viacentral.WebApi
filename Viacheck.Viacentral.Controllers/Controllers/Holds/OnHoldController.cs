using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using Viam.Framework.Logger;
using Viacheck.Viacentral.Business;
using Viacheck.Viacentral.Models;
using Viacheck.Viacentral.Models.Holds;
using Viacheck.Viacentral.Controllers.Constants;
using Viacheck.Viacentral.Models.Viacheck;

namespace Viacheck.Viacentral.Controllers.Controllers.Holds
{
    [Route("api/OnHold")]
    [ApiController]
    [EnableCors("AllowOrigin")]

    public class OnHoldController : Controller
    {
        private OnHoldBusiness _onHoldBussines;
        private readonly IOptions<ConfigurationModel> _configurations;
        private readonly IViamLogger _viamLogger;

        public OnHoldController(IOptions<ConfigurationModel> configurations, IViamLogger log)
        {
            _configurations = configurations;
            _viamLogger = log;
            _onHoldBussines = new OnHoldBusiness(_configurations.Value);         
        }

        [HttpPost]
        [Route("GetChecksOnHold")]
        public async Task<OnHoldResponseModel> GetChecksOnHold([FromBody]OnHoldFiltersModels holdParameters)
        {
            try
            {
                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel { Code = MessageResponse.SUCCESS_CODE },
                    HoldResults = _onHoldBussines.GetChecksOnHold(holdParameters)
            };
              
            }
            catch (Exception ex)
            {
                _viamLogger.Error(ex);
                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel { Code = MessageResponse.ERROR_CODE, Message = ex.Message }
                };
            }
        }

        [HttpPost]
        [Route("GetChecksOnHoldV1")]
        public async Task<OnHoldResponseModel> GetChecksOnHoldV1([FromBody]OnHoldFiltersModels holdParameters)
        {
            try
            {
                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel { Code = MessageResponse.SUCCESS_CODE },
                    HoldResults = _onHoldBussines.GetChecksOnHoldV1(holdParameters)
                };

            }
            catch (Exception ex)
            {
                _viamLogger.Error(ex);
                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel { Code = MessageResponse.ERROR_CODE, Message = ex.Message }
                };
            }
        }


        [HttpPost]
        [Route("ReleaseChecks")]
        public async Task<OnHoldResponseModel> ReleaseChecks([FromBody]OnHoldReleaseModel holdParameters)
        {
            try
            {
                var checkListResult=_onHoldBussines.ReleaseChecks(holdParameters.CheckList, holdParameters.User, holdParameters.Operation);
                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel { Code = MessageResponse.SUCCESS_CODE },
                    HoldResults = checkListResult

                };

            }
            catch (Exception ex)
            {
                _viamLogger.Error(ex);
                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel { Code = MessageResponse.ERROR_CODE, Message = ex.Message }
                };
            }
        }

        [HttpPost]
        [Route("UpdateCheckInformation")]
        public async Task<OnHoldResponseModel> UpdateCheckInformation([FromBody]OnHoldCheckUpdateModel holdParameters)
        {
            try
            {
                
                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel { Code = MessageResponse.SUCCESS_CODE
                    ,Message = _onHoldBussines.UpdateCheckInformation(holdParameters.Check, holdParameters.User)
            },

                };

            }
            catch (Exception ex)
            {
                _viamLogger.Error(ex);
                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel { Code = MessageResponse.ERROR_CODE, Message = ex.Message }
                };
            }
        }

        [HttpPost]
        [Route("GetAccountInformation")]
        public async Task<OnHoldResponseModel> GetAccountInformation([FromBody]OnHoldChecksModel checkParameters)
        {
            try
            {

                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel
                    {
                        Code = MessageResponse.SUCCESS_CODE                    
                       
                    },
                    AccountInformation= _onHoldBussines.GetAccountInformation(checkParameters)

                };

            }
            catch (Exception ex)
            {
                _viamLogger.Error(ex);
                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel { Code = MessageResponse.ERROR_CODE, Message = ex.Message }
                };
            }
        }

        [HttpGet]
        [Route("GetCategorySummary")]
        public async Task<OnHoldResponseModel> GetCategorySummary()
        {
            try
            {

                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel
                    {
                        Code = MessageResponse.SUCCESS_CODE

                    },
                    CategorySummary = _onHoldBussines.GetCategorySummary()

                };

            }
            catch (Exception ex)
            {
                _viamLogger.Error(ex);
                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel { Code = MessageResponse.ERROR_CODE, Message = ex.Message }
                };
            }
        }


        [HttpGet]
        [Route("GetOnHoldLegend")]
        public async Task<OnHoldResponseModel> GetOnHoldLegend()
        {
            try
            {

                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel
                    {
                        Code = MessageResponse.SUCCESS_CODE

                    },
                    OnHoldLegend = _onHoldBussines.GetOnHoldLegend()

                };

            }
            catch (Exception ex)
            {
                _viamLogger.Error(ex);
                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel { Code = MessageResponse.ERROR_CODE, Message = ex.Message }
                };
            }
        }

        /// <summary>
        /// Get agency summary
        /// </summary>
        /// <returns></returns>
        ///    [HttpGet]
        [Route("GetAgencySummary")]
        public OnHoldResponseModel GetAgencySummary()
        {
            try
            {

                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel
                    {
                        Code = MessageResponse.SUCCESS_CODE

                    },
                    AgencySummary = _onHoldBussines.GetAgencySummary()

                };

            }
            catch (Exception ex)
            {
                _viamLogger.Error(ex);
                return new OnHoldResponseModel()
                {
                    Status = new MessageResponseModel { Code = MessageResponse.ERROR_CODE, Message = ex.Message }
                };
            }
          
        }


    }
}