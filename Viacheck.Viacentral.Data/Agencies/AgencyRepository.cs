using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Viacheck.Viacentral.Models.Agencies;

namespace Viacheck.Viacentral.Agencies
{
    public class AgencyRepository
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
        public AgencyRepository(string connection)
        {
            _connectionString = connection;
        }
        #endregion

        /// <summary>
        /// Get Agency list
        /// </summary>
        /// <returns></returns>
        public List<AgencyModel> GetAllAgencies()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var agencyList = new List<AgencyModel>();
                try
                {
                    string sql = "Viacheck.PCN_GetAllAgencies";

                    agencyList = connection.Query<AgencyModel>(sql, new DynamicParameters(), commandType: CommandType.StoredProcedure).ToList();

                    return agencyList;

                }
                catch (Exception ex)
                {
                    return null;

                }
            }
        }
    }
}
