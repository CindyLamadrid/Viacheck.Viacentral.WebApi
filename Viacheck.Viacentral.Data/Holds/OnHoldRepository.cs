using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Viacheck.Viacentral.Models.Holds;
using Viacheck.Viacentral.Models.Viacheck;


namespace Viacheck.Viacentral.Data.Holds
{

    public class OnHoldRepository
    {

        #region  Variables and props
        private string _connectionString;

        /// <summary>
        /// Property to manage database connection.
        /// </summary>
        /// <value></value>
        private IDbConnection Connection => new SqlConnection(_connectionString);

        /// <summary>
        /// Property to manage database connection.
        /// </summary>
        /// <value></value>
        private SqlConnection BatchConnection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        #endregion
        #region Constructor
        public OnHoldRepository(string connection)
        {
            _connectionString = connection;
        }
        #endregion
        /// <summary>
        /// Getmultiples holds by rol
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="categoryHold">Category Id</param>
        /// <param name="dateFrom">Date from</param>
        /// <param name="dateTo">Date To</param>
        /// <param name="amount">Amount</param>
        /// <param name="user">User login</param>
        /// <returns>Hold results</returns>
        public List<OnHoldChecksModel> GetMultiplesHoldsData(OnHoldFiltersModels filters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var holdList = new List<OnHoldChecksModel>();

                string sql = "Viacheck.PCN_GetChecksMultipleHolds";
                var p = new DynamicParameters();
                p.Add("@agencyCode", filters.AgencyCode, dbType: DbType.Int32, direction: ParameterDirection.Input);
                p.Add("@groupHold", filters.CategoryHold, dbType: DbType.Int32, direction: ParameterDirection.Input);
                p.Add("@dateFrom", filters.DateFrom, dbType: DbType.DateTime, direction: ParameterDirection.Input);
                p.Add("@dateTo", filters.DateTo, dbType: DbType.DateTime, direction: ParameterDirection.Input);
                p.Add("@amount", filters.Amount, dbType: DbType.Decimal, direction: ParameterDirection.Input);
                p.Add("@user", filters.User, dbType: DbType.String, direction: ParameterDirection.Input);

                holdList = connection.Query<OnHoldChecksModel>(sql, p, commandType: CommandType.StoredProcedure).ToList();

                return holdList;

            }
        }

        /// <summary>
        /// Get multiples holds by rol
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="categoryHold">Category Id</param>
        /// <param name="dateFrom">Date from</param>
        /// <param name="dateTo">Date To</param>
        /// <param name="amount">Amount</param>
        /// <param name="user">User login</param>
        /// <returns>Hold results</returns>
        public List<OnHoldChecksModel> GetMultiplesHoldsDataV1(OnHoldFiltersModels filters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var holdList = new List<OnHoldChecksModel>();

                string sql = "Viacheck.PCN_GetChecksMultipleHolds_V1";
                var p = new DynamicParameters();
                p.Add("@agencyCode", filters.AgencyCode, dbType: DbType.Int32, direction: ParameterDirection.Input);
                p.Add("@groupHold", filters.CategoryHold, dbType: DbType.Int32, direction: ParameterDirection.Input);
                p.Add("@dateFrom", filters.DateFrom, dbType: DbType.DateTime, direction: ParameterDirection.Input);
                p.Add("@dateTo", filters.DateTo, dbType: DbType.DateTime, direction: ParameterDirection.Input);
                p.Add("@amount", filters.Amount, dbType: DbType.Decimal, direction: ParameterDirection.Input);               
                p.Add("@pageSize", filters.PageSize, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@pageNum", filters.PageNum, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@idHoldLegend", string.IsNullOrEmpty(filters.IdHoldLegend)?null: filters.IdHoldLegend, dbType: DbType.String, direction: ParameterDirection.Input);

                holdList = connection.Query<OnHoldChecksModel>(sql, p, commandType: CommandType.StoredProcedure).ToList();

                return holdList;
            }
        }


        /// <summary>
        /// Release or rejects checks
        /// </summary>
        /// <param name="checkList"></param>
        /// <param name="user"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public List<OnHoldChecksModel> ReleaseChecks(OnHoldChecksModelList checkList, string user, string operation)
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectionString))
            {
                SqlCommand command = null;
                try
                {
                    dbConnection.Open();
                    command = new SqlCommand("Viacheck.PCN_ReleaseChecksOnHold", dbConnection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@Checks", System.Data.SqlDbType.Structured);
                    command.Parameters["@Checks"].Value = checkList;

                    command.Parameters.Add("@ProcessedUser", System.Data.SqlDbType.VarChar);
                    command.Parameters["@ProcessedUser"].Value = user;

                    command.Parameters.Add("@Operation", System.Data.SqlDbType.VarChar);
                    command.Parameters["@Operation"].Value = operation;

                    command.CommandTimeout = 0;
                    var reader = command.ExecuteReader();
                    var validationList = new List<OnHoldChecksModel>();

                    while (reader.Read())
                    {

                        validationList.Add(new OnHoldChecksModel
                        {
                            CheckID = int.Parse(reader["CheckId"].ToString()),
                            HoldDescription = (reader["HoldDescription"].ToString())

                        });
                    }

                    return validationList;
                }
                catch (Exception ex)
                {
                    if (command != null)
                    {
                        command.Cancel();
                        command.Dispose();
                    }
                    throw ex;

                }
                finally
                {
                    if (dbConnection != null)
                    {
                        if (dbConnection.State == ConnectionState.Open)
                            dbConnection.Close();

                    }
                }


            }
        }


        /// <summary>
        /// Update check Information
        /// </summary>
        /// <param name="check"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string UpdateCheckInformation(OnHoldChecksModel check, string user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string response = string.Empty;

                string sql = "Viacheck.PCN_UpdateCheckOnHoldInfo";
                var p = new DynamicParameters();
                p.Add("@AMOUNT", check.Amount, dbType: DbType.Int64, direction: ParameterDirection.Input);
                p.Add("@ACCOUNT", check.Account, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@CHECKID", check.CheckID, dbType: DbType.Int32, direction: ParameterDirection.Input);
                p.Add("@CHECK_NUMBER", check.CheckNumber, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@ABA", check.Transit, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@USERNAME", user, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@ISVCP", false, dbType: DbType.Boolean, direction: ParameterDirection.Input);
                p.Add("@error", string.Empty, dbType: DbType.String, direction: ParameterDirection.Output);

                connection.Execute(sql, p, commandType: CommandType.StoredProcedure);
                response = p.Get<string>("@error");
                return response;

            }
        }

        /// <summary>
        /// Update notes in crm for each check
        /// </summary>
        /// <param name="check"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string UpdateCheckNotes(OnHoldChecksModel check, string user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string response = string.Empty;

                string sql = "dbo.spAddMessageByCheckToCRM";
                var p = new DynamicParameters();
                p.Add("@checkId", check.CheckID, dbType: DbType.Int32, direction: ParameterDirection.Input);
                p.Add("@message", check.Notes, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@username", user, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@error", string.Empty, dbType: DbType.String, direction: ParameterDirection.Output);

                connection.Execute(sql, p, commandType: CommandType.StoredProcedure);
                response = p.Get<string>("@error");
                return response;

            }
        }

        /// <summary>
        /// GEt Account informaion
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public AccountInformationModel GetAccountInformation(OnHoldChecksModel check)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var accountInformation = new AccountInformationModel();

                string sql = "dbo.PCN_GET_ACCOUNTINFORMATION_HOLDCHECKS";
                var p = new DynamicParameters();
                p.Add("@Account", check.Account, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@Location", check.LocationId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                p.Add("@Deposit", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Returned", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Porcent", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@DepositLastDay", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@ReturnedLastDay", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@TimeAgency", 0, dbType: DbType.DateTime, direction: ParameterDirection.Output);
                p.Add("@TimeMaker", 0, dbType: DbType.DateTime, direction: ParameterDirection.Output);

                var result = connection.Query<AccountInformationModel>(sql, p, commandType: CommandType.StoredProcedure).ToList();
                if (result != null)
                {
                    accountInformation = result.FirstOrDefault();
                }
                return accountInformation;

            }
        }

        /// <summary>
        /// Get category Summary 
        /// </summary>
        /// <returns></returns>
        public List<CategorySummaryModel> GetCategorySummary()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var categoryInformation = new List<CategorySummaryModel>();

                string sql = "[Viacheck].[PCN_GetSummaryOnHolds]";
                var p = new DynamicParameters();
                categoryInformation = connection.Query<CategorySummaryModel>(sql, p, commandType: CommandType.StoredProcedure).ToList();

                return categoryInformation;
            }

        }

        public List<OnHoldLegendModel> GetOnHoldLegend()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var holdLegend = new List<OnHoldLegendModel>();
                string sql = "Viacheck.PCN_GetOnHoldLegend";
                var p = new DynamicParameters();
                holdLegend = connection.Query<OnHoldLegendModel>(sql, p, commandType: CommandType.StoredProcedure).ToList();

                return holdLegend;
            }
        }


        public List<AgencySummaryModel> GetAgencySummary()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var holdLegend = new List<AgencySummaryModel>();

                string sql = "Viacheck.PCN_GetSummaryAgencyOnHold";
                var p = new DynamicParameters();
                holdLegend = connection.Query<AgencySummaryModel>(sql, p, commandType: CommandType.StoredProcedure).ToList();

                return holdLegend;

            }
        }
    }
}
