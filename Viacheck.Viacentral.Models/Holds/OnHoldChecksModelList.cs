using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.SqlServer.Server;

namespace Viacheck.Viacentral.Models.Viacheck
{
    public class OnHoldChecksModelList : List<OnHoldChecksModel>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var row = new SqlDataRecord(
                new SqlMetaData("CheckId", SqlDbType.Int),
                new SqlMetaData("Status", SqlDbType.SmallInt)
                );

            foreach (OnHoldChecksModel check in this)
            {
                row.SetInt32(0, check.CheckID);
                row.SetInt16(1, Int16.Parse( check.CheckStatus.ToString()));
                yield return row;
            }
        }
    }
}
