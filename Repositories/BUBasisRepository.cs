using System.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;
using MESWebAPI.Models;
namespace MESWebAPI.Repositories
{
#nullable disable
    public class BUBasisRepository : Repository
    {
        public BUBasisRepository(string connectionString) : base(connectionString)
        { }

        public BUBasisRepository() : base()
        { }

        public int AddPlantBOPWorkStation(string guid, string productCode, string stationName, string specName, string processNOLevel, string bopid)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ID", guid);
            dp.Add("@ProductCode", productCode);
            dp.Add("@StationName", stationName);
            dp.Add("@specName", specName);
            dp.Add("@ProcessNOLevel", processNOLevel);
            dp.Add("@BOPID", bopid);

            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return conn.ExecuteScalar<int>("usp_ZTABLE_PlantBOP_WorkStation_Insert", dp, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<ViewerConfigInfo> GetViewerConfig(int productID, byte enable)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ProductID", productID);
            dp.Add("@Enable", enable);

            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return conn.Query<ViewerConfigInfo>("usp_ViewerConfigInfo_Query", dp, commandType: CommandType.StoredProcedure);
            }
        }
    }
}