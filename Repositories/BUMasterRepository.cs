using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MESWebAPI.Models;

namespace MESWebAPI.Repositories
{
    public class BUMasterRepository : Repository
    {
        public BUMasterRepository(string connectionString) : base(connectionString)
        { }

        public BUMasterRepository() : base()
        { }

        //
        public IEnumerable<RepairUser> GetRepairUserInfo(int productID, byte status, DateTime startTime, DateTime endTime)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ProductID", productID);
            dp.Add("@Status", status);
            dp.Add("@StartTime", startTime);
            dp.Add("@EndTime", endTime);

            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return conn.Query<RepairUser>("usp_scada_RepairUserQuery", dp, commandType: CommandType.StoredProcedure);
            }
        }
    }
}