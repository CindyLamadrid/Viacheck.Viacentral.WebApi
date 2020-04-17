using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Viacheck.Viacentral.Data.Holds;
using Viacheck.Viacentral.Models;
using Viacheck.Viacentral.Models.Holds;
using Viacheck.Viacentral.Models.Viacheck;

namespace Viacheck.Viacentral.Business
{
    public class OnHoldBusiness
    {
        private OnHoldRepository _onHoldRepositoryRead;
        private OnHoldRepository _onHoldRepositoryWrite;

        public OnHoldBusiness(ConfigurationModel configuration)
        {
            _onHoldRepositoryRead = new OnHoldRepository(configuration.ViacheckRead);
            _onHoldRepositoryWrite = new OnHoldRepository(configuration.ViacheckWriter);
        }

        public List<OnHoldChecksModel> GetChecksOnHold(OnHoldFiltersModels filters)
        {
            var checkHoldList = new List<OnHoldChecksModel>();
            var checkIdsList = _onHoldRepositoryRead.GetMultiplesHoldsData(filters);

            var checkList = (from chk in checkIdsList
                             select chk.CheckID).Distinct();

            foreach (var checkId in checkList.ToList())
            {
                var checkHoldsList = (from chk in checkIdsList
                                      where chk.CheckID == checkId
                                      select chk
                                ).ToList();


                var check = checkHoldsList.FirstOrDefault();
                var arrayStatusDescription = this.GetDescriptionCheck(check.Description.ToString());
                var isDefinitive = arrayStatusDescription.Length == 3 ? arrayStatusDescription[1] : string.Empty;
                List<OnHoldDescriptionModel> holds = this.GetHoldsByCheck(checkHoldsList, isDefinitive);

                string holdColor = "#000000";
                if (isDefinitive == "1")
                {
                    holdColor = "red";
                }
                else
                {
                    var holdPriority = (from obj in checkHoldsList select obj).Min(c => c.HoldPriority);
                    holdColor = (from obj in checkHoldsList where obj.HoldPriority == holdPriority select obj).FirstOrDefault().HoldColor;
                }

                checkHoldList.Add(new OnHoldChecksModel()
                {
                    CheckID = check.CheckID,
                    BatchID = check.BatchID,
                    CheckStatus = check.CheckStatus,
                    Amount = check.Amount,
                    Transit = check.Transit,
                    CheckNumber = check.CheckNumber,
                    Account = check.Account,
                    Created = check.Created,
                    LocationName = check.LocationName,
                    RefImageKey = check.RefImageKey,
                    Notes = check.Notes,
                    OwnLicense = check.OwnLicense,
                    FinCen = check.FinCen,
                    Holds = holds,
                    HighlightedField = check.HighlightedField,
                    LocationId = check.LocationId,
                    GiactValidation = check.GiactValidation.ToString(),
                    Description = DescriptionHelper(arrayStatusDescription, check.Description),
                    IsDefinitive = isDefinitive,
                    HoldColor = holdColor,
                    VerifyHoldNoShowed = (holds.Count != checkHoldsList.Count),
                    OriginalAccount = check.Account,
                    OriginalAmount = check.Amount,
                    OriginalCheckNumber = check.CheckNumber,
                    OriginalTransit = check.Transit

                });

            }
            return checkHoldList;
        }


        public List<OnHoldChecksModel> GetChecksOnHoldV1(OnHoldFiltersModels filters)
        {

            var checkHoldList = _onHoldRepositoryRead.GetMultiplesHoldsDataV1(filters);
            return checkHoldList;
        }

        /// <summary>
        /// Get check Description
        /// </summary>
        /// <returns></returns>
        private string[] GetDescriptionCheck(string rawDesc)
        {
            string[] dataFromDesc;
            var splittedStatus = rawDesc.Split(char.Parse("|"));

            if (splittedStatus.Length == 3)
            {
                if (splittedStatus[0].Trim() != splittedStatus[2].Trim())
                {
                    splittedStatus[0] += " " + splittedStatus[2];
                }
                return splittedStatus;
            }
            else
            {
                dataFromDesc = new string[1];
                dataFromDesc[0] = rawDesc;

                return dataFromDesc;
            }
        }

        /// <summary>
        /// Get holds by checs
        /// </summary>
        /// <param name="check">check</param>
        /// <param name="isDefinitive">If hte cehck is or not definitive</param>
        /// <returns></returns>
        private List<OnHoldDescriptionModel> GetHoldsByCheck(List<OnHoldChecksModel> check, string isDefinitive)
        {
            List<OnHoldDescriptionModel> holdList = new List<OnHoldDescriptionModel>();

            foreach (var hold in check)
            {
                if (hold.ShowDescription)
                {
                    holdList.Add(this.AddHoldToList(hold));
                }
                else
                {
                    var holdsToShow = (from obj in check where obj.ShowDescription select obj).ToList();
                    if (holdsToShow != null)
                    {
                        if (holdsToShow.Count == 0)
                        {
                            holdList.Add(this.AddHoldToList(hold));
                        }
                        else
                        {
                            if (isDefinitive == "0")
                            {
                                holdsToShow = (from obj in check where obj.HoldPriority < hold.HoldPriority && obj.NamePriority != "basic_hold" && obj.ShowDescription select obj).ToList();
                                if (holdsToShow != null)
                                {
                                    if (holdsToShow.Count > 0)
                                    {
                                        holdList.Add(this.AddHoldToList(hold));
                                    }
                                }

                            }
                            else
                            {
                                holdList.Add(this.AddHoldToList(hold));
                            }
                        }
                    }
                }
            }
            return holdList;
        }

        /// <summary>
        /// Add hold list.
        /// </summary>
        /// <param name="hold"></param>
        /// <returns></returns>
        private OnHoldDescriptionModel AddHoldToList(OnHoldChecksModel hold)
        {
            return new OnHoldDescriptionModel()
            {
                IdHold = hold.IdHold.ToString(),
                NameHold = hold.NameHold,
                IdGroupHold = hold.IdGroupHold
            };
        }

        /// <summary>
        /// GEt if check if warning or denifinitve
        /// </summary>
        /// <param name="arrDescription"></param>
        /// <param name="fullDescription"></param>
        /// <returns></returns>
        private string DescriptionHelper(string[] arrDescription, string fullDescription)
        {
            string descriptiveWarning;

            switch (arrDescription[1].Trim())
            {
                case "2":
                    descriptiveWarning = "-Warning.";
                    break;
                case "1":
                    descriptiveWarning = "-Definitive.";
                    break;
                default:
                    descriptiveWarning = string.Empty;
                    break;
            }

            if (arrDescription.Length == 3)
            {
                var response = string.Format("{0},{1}", arrDescription[0], descriptiveWarning).Replace(@",", " ");
                return response;
            }
            else
            {
                return fullDescription;
            }
        }

        /// <summary>
        /// Release checks
        /// </summary>
        /// <param name="checkList"></param>
        /// <param name="user"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public List<OnHoldChecksModel> ReleaseChecks(OnHoldChecksModelList checkList, string user, string operation)
        {

            var checkListResult = _onHoldRepositoryWrite.ReleaseChecks(checkList, user, operation);
            /// delete from notification center

            return checkListResult;
        }

        /// <summary>
        /// Update check Information
        /// </summary>
        /// <param name="check"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string UpdateCheckInformation(OnHoldChecksModel check, string user)
        {
            if (!string.IsNullOrEmpty(check.Notes))
            {
                _onHoldRepositoryWrite.UpdateCheckNotes(check, user);
            }

            return _onHoldRepositoryWrite.UpdateCheckInformation(check, user);
        }


        /// <summary>
        /// Get Account informaion
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public AccountInformationModel GetAccountInformation(OnHoldChecksModel check)
        {
            return _onHoldRepositoryRead.GetAccountInformation(check);
        }


        /// <summary>
        /// Get Account informaion
        /// </summary>      
        /// <returns></returns>
        public List<CategorySummaryModel> GetCategorySummary()
        {
            return _onHoldRepositoryRead.GetCategorySummary();
        }


        /// <summary>
        /// Get Account informaion
        /// </summary>      
        /// <returns></returns>
        public List<OnHoldLegendModel> GetOnHoldLegend()
        {
            return _onHoldRepositoryRead.GetOnHoldLegend();
        }


        /// <summary>
        /// Get agency summary
        /// </summary>
        /// <returns></returns>
        public List<AgencySummaryModel> GetAgencySummary()
        {
            return _onHoldRepositoryRead.GetAgencySummary();
        }
    }
}
