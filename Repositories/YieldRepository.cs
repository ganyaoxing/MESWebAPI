
using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MESWebAPI.Models;
using MySql.Data.MySqlClient;

namespace MESWebAPI.Repositories
{
#nullable disable
    public class YieldRepository : Repository
    {
        public YieldRepository(string connectionString) : base(connectionString)
        { }

        public YieldRepository() : base()
        { }
        #region 修复逻辑yield
        public IEnumerable<YieldReport> GetYieldData(int productID, int viewerConfigID, DateTime startTime, DateTime endTime, string lineID, string reportType, string processNO, MySqlConnection mySqlConnection, ref string msg)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@p_ViewerConfigID", viewerConfigID);
            dp.Add("@p_BeginDate", startTime);
            dp.Add("@p_EndDate", endTime);
            dp.Add("@p_LINEID", lineID);
            dp.Add("@p_ReportType", reportType);
            dp.Add("@p_ProcessNO", processNO);
            msg = string.Format("Call sp_VPY_Repair_YieldReportQueryProcess('{0}','{1}',{2},'{3}','{4}','{5}')",
                            startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"), viewerConfigID, lineID, reportType, processNO);
            using (IDbConnection conn = mySqlConnection)
            {
                conn.Open();
                return conn.Query<YieldReport>("sp_VPY_Repair_YieldReportQueryProcess", dp, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<YieldReportDetail> GetYieldDetailData(int productID, int viewerConfigID, DateTime startTime, DateTime endTime, string lineID,
        string reportType, string processNO,
        int reTestCount, int failCount, int wipCount, string conditionID, string stationID, int firstCount, MySqlConnection mySqlConnection, ref string msg)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@p_ViewerConfigID", viewerConfigID);
            dp.Add("@p_BeginDate", startTime);
            dp.Add("@p_EndDate", endTime);
            dp.Add("@p_LINEID", lineID);
            dp.Add("@p_ReportType", reportType);
            dp.Add("@p_ProcessNO", processNO);

            dp.Add("@p_ReTestCount", reTestCount);
            dp.Add("@p_FailCount", failCount);
            dp.Add("@p_WIPCount", wipCount);
            if (!string.IsNullOrEmpty(conditionID))
                dp.Add("@p_ConditionID", conditionID);
            else
                dp.Add("@p_ConditionID", null);
            dp.Add("@p_StationID", stationID);
            dp.Add("@p_FirstCount", firstCount);

            if (string.IsNullOrEmpty(conditionID))
            {
                msg = string.Format("Call sp_Repair_YieldReportCountDetail_Query2Process({0},{1},{2},{3},{4},null,'{5}','{6}','{7}','{8}','{9}','{10}')",
                   viewerConfigID, firstCount,
                  reTestCount, failCount, wipCount,
                  startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"),
                  stationID, lineID, reportType, processNO);
            }
            else
            {
                msg = string.Format("Call sp_Repair_YieldReportCountDetail_Query2Process({0},{1},{2},{3},{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                   viewerConfigID, firstCount,
                  reTestCount, failCount, wipCount,
                  conditionID, startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"),
                  stationID, lineID, reportType, processNO);
            }
            using (IDbConnection conn = mySqlConnection)
            {
                conn.Open();
                return conn.Query<YieldReportDetail>("sp_Repair_YieldReportCountDetail_Query2Process", dp, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<YieldReportDetailChart> GetYieldDetailChartData(int productID, int viewerConfigID, DateTime startTime, DateTime endTime, string lineID,
        string reportType, string processNO,
        int reTestCount, int failCount, int wipCount, string conditionID, string stationID, int firstCount, string failType, MySqlConnection mySqlConnection, ref string msg)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@p_ViewerConfigID", viewerConfigID);
            dp.Add("@p_BeginDate", startTime);
            dp.Add("@p_EndDate", endTime);
            dp.Add("@p_LINEID", lineID);
            dp.Add("@p_ReportType", reportType);
            dp.Add("@p_ProcessNO", processNO);

            dp.Add("@p_ReTestCount", reTestCount);
            dp.Add("@p_FailCount", failCount);
            dp.Add("@p_WIPCount", wipCount);
            if (!string.IsNullOrEmpty(conditionID))
                dp.Add("@p_ConditionID", conditionID);
            else
                dp.Add("@p_ConditionID", null);
            dp.Add("@p_StationID", stationID);
            dp.Add("@p_FirstCount", firstCount);
            dp.Add("@p_FailType", failType);

            if (string.IsNullOrEmpty(conditionID))
            {
                msg = string.Format("Call sp_Repair_YieldReportCountDetail_QueryQtyProcess({0},{1},{2},{3},{4},null,'{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                   viewerConfigID, firstCount,
                  reTestCount, failCount, wipCount,
                  startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"),
                  stationID, lineID, reportType, failType, processNO);
            }
            else
            {
                msg = string.Format("Call sp_Repair_YieldReportCountDetail_QueryQtyProcess({0},{1},{2},{3},{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
                   viewerConfigID, firstCount,
                  reTestCount, failCount, wipCount,
                  conditionID, startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"),
                  stationID, lineID, reportType, failType, processNO);
            }
            using (IDbConnection conn = mySqlConnection)
            {
                conn.Open();
                return conn.Query<YieldReportDetailChart>("sp_Repair_YieldReportCountDetail_QueryQtyProcess", dp, commandType: CommandType.StoredProcedure);
            }
        }
        #endregion

        #region Rework yield
        public IEnumerable<YieldReport> GetYieldReworkData(int productID, int viewerConfigID, DateTime startTime, DateTime endTime, string lineID, string processNO, MySqlConnection mySqlConnection, ref string msg)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@p_ViewerConfigID", viewerConfigID);
            dp.Add("@p_BeginDate", startTime);
            dp.Add("@p_EndDate", endTime);
            dp.Add("@p_LINEID", lineID);
            dp.Add("@p_ProcessNO", processNO);
            msg = $"Call sp_Rework_YieldReportQueryProcess('{startTime.ToString("yyyy-MM-dd HH:mm:ss")}','{endTime.ToString("yyyy-MM-dd HH:mm:ss")}',{viewerConfigID},'{lineID}','{processNO}')";
            using (IDbConnection conn = mySqlConnection)
            {
                conn.Open();
                return conn.Query<YieldReport>("sp_Rework_YieldReportQueryProcess", dp, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<YieldReportDetail> GetYieldReworkDetailData(int productID, int viewerConfigID, DateTime startTime, DateTime endTime, string lineID,
         string processNO, int failCount, int wipCount, string conditionID, string stationID, int passCount, MySqlConnection mySqlConnection, ref string msg)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@p_ViewerConfigID", viewerConfigID);
            dp.Add("@p_PassCount", passCount);
            dp.Add("@p_FailCount", failCount);
            dp.Add("@p_WIPCount", wipCount);
            dp.Add("@p_ConditionID", string.IsNullOrEmpty(conditionID) ? null : conditionID);
            dp.Add("@p_BeginDate", startTime);
            dp.Add("@p_EndDate", endTime);
            dp.Add("@p_StationID", stationID);
            dp.Add("@p_LINEID", lineID);
            dp.Add("@p_ProcessNO", processNO);

            //dp.Add("@p_ReTestCount", reTestCount);
            //dp.Add("@p_FirstCount", firstCount);
            msg = $"Call sp_Rework_YieldReportCountDetail_Query2Process({viewerConfigID},{passCount},{failCount},{wipCount},null,'{startTime.ToString("yyyy-MM-dd HH:mm:ss")}','{endTime.ToString("yyyy-MM-dd HH:mm:ss")}','{stationID}','{lineID}','{processNO}')";
            using (IDbConnection conn = mySqlConnection)
            {
                conn.Open();
                return conn.Query<YieldReportDetail>("sp_Rework_YieldReportCountDetail_Query2Process", dp, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<YieldReportDetailChart> GetYieldReworkDetailChartData(int productID, int viewerConfigID, DateTime startTime, DateTime endTime, string lineID,
         string processNO, int failCount, int wipCount, string conditionID, string stationID, int passCount, string failType, MySqlConnection mySqlConnection, ref string msg)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@p_ViewerConfigID", viewerConfigID);
            dp.Add("@p_PassCount", passCount);
            dp.Add("@p_FailCount", failCount);
            dp.Add("@p_WIPCount", wipCount);
            dp.Add("@p_ConditionID", string.IsNullOrEmpty(conditionID) ? null : conditionID);
            dp.Add("@p_BeginDate", startTime);
            dp.Add("@p_EndDate", endTime);
            dp.Add("@p_StationID", stationID);
            dp.Add("@p_LINEID", lineID);
            dp.Add("@p_FailType", failType);
            dp.Add("@p_ProcessNO", processNO);
            msg = $"Call sp_Rework_YieldReportCountDetail_QueryQtyProcess({viewerConfigID},{passCount},{failCount},{wipCount},null,'{startTime.ToString("yyyy-MM-dd HH:mm:ss")}','{endTime.ToString("yyyy-MM-dd HH:mm:ss")}','{stationID}','{lineID}','{failType}','{processNO}')";
            using (IDbConnection conn = mySqlConnection)
            {
                conn.Open();
                return conn.Query<YieldReportDetailChart>("sp_Rework_YieldReportCountDetail_QueryQtyProcess", dp, commandType: CommandType.StoredProcedure);
            }
        }
        #endregion

        public BUMariaDBConnection GetProductMariaDBConnection(int productID)
        {
            string tSql = @"SELECT  ISNULL(c.[YieldDB],d.[YieldDB]) YieldDB,ISNULL(c.[YieldTestDB],d.[YieldTestDB]) YieldTestDB,
                                    a.ProductID,a.ProductName,b.BUID
	                            FROM tProduct a
	                            INNER JOIN tBU_NBA  b ON a.BUID=b.BUID
	                            LEFT JOIN tBUMariaDBConnection c ON a.ProductID=c.ProductID
	                            LEFT JOIN tBUMariaDBConnection d ON b.BUID=d.BUID AND d.ProductID=-1
							    WHERE a.ProductID=@ProductID";
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return conn.QueryFirstOrDefault<BUMariaDBConnection>(tSql, new { ProductID = productID });
            }
        }
    }
}