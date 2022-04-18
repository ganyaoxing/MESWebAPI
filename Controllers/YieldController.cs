using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using MESWebAPI.Repositories;
using MESWebAPI.Tools;
using MESWebAPI.Models;
using MySql.Data.MySqlClient;

namespace MESWebAPI.Controllers
{
#nullable disable
    [ApiController]
    [Route("[controller]")]
    public class YieldController : ControllerBase
    {
        protected readonly IMemoryCache _memoryCache;
        protected readonly ILogger<MESControllerBase> _logger;
        protected readonly IConfiguration _config;
        protected YieldRepository _yieldRepository;

        public YieldController(ILogger<MESControllerBase> logger, IConfiguration config, IMemoryCache iMemoryCache)
        {
            _logger = logger;
            _config = config;
            _memoryCache = iMemoryCache;
            _yieldRepository = new YieldRepository(_config["ConnectionStrings:BasisConnection"]);
        }

        private MySqlConnection GetMysqlConnection(int productID, EnumList.EnvironmentList environment = EnumList.EnvironmentList.Release)
        {
            MySqlConnection mysqlConnection;
            string key = $"{CacheKey.ProductMariaDBConnPrefix}-{productID.ToString()}-{environment.ToString()}";
            if (!_memoryCache.TryGetValue(key, out mysqlConnection))
            {
                BUMariaDBConnection mariaDBConnection = _yieldRepository.GetProductMariaDBConnection(productID);
                if (mariaDBConnection == null || string.IsNullOrEmpty(mariaDBConnection.YieldDB))
                {
                    throw new Exception($"Failed to find MySQL connection for productID {productID}");
                }
                if (environment == EnumList.EnvironmentList.Testing)
                    mysqlConnection = new MySqlConnection(mariaDBConnection.YieldTestDB);
                else
                    mysqlConnection = new MySqlConnection(mariaDBConnection.YieldDB);
                _memoryCache.Set(key, mysqlConnection, new TimeSpan(1, 0, 0));
            }
            return mysqlConnection;
        }

        #region 修复逻辑yield
        [HttpGet]
        [Route("YieldReport")]
        public ReturnInfoLayUI<YieldReport> GetYieldReportInfo(int productID, DateTime startTime, DateTime endTime, int viewerConfigID = 64
        , string lineID = "ALL", string reportType = "A", string processNO = "")
        {
            try
            {
                MySqlConnection mySqlConnection = GetMysqlConnection(productID);
                IEnumerable<YieldReport> lstYieldReport;
                string key = CacheKey.RepairYield + $"{productID}{startTime}{endTime}{viewerConfigID}{lineID}{reportType}{processNO}";
                string keyMsg = key + "Msg";
                string msg = string.Empty;
                _memoryCache.TryGetValue(keyMsg, out msg);
                if (!_memoryCache.TryGetValue(key, out lstYieldReport))
                {
                    lstYieldReport = _yieldRepository.GetYieldData(productID, viewerConfigID, startTime, endTime, lineID, reportType, processNO, mySqlConnection, ref msg);
                    _memoryCache.Set(key, lstYieldReport, new TimeSpan(0, 0, 30));
                    _memoryCache.Set(keyMsg, msg, new TimeSpan(0, 0, 60));
                }
                return new ReturnInfoLayUI<YieldReport>(msg, lstYieldReport);
            }
            catch (Exception ex)
            {
                return new ReturnInfoLayUI<YieldReport>(ex.ToString(), 1);
            }
        }

        [HttpGet]
        [Route("YieldReportDetail")]
        public ReturnInfoLayUI<YieldReportDetail> GetYieldReportDetailInfo(int productID, DateTime startTime, DateTime endTime, int viewerConfigID = 64
                , string lineID = "ALL", string reportType = "A", string processNO = "",
                int reTestCount = 0, int failCount = 0, int wipCount = 0, string conditionID = "", string stationID = "FM7010", int firstCount = 1)
        {
            try
            {
                MySqlConnection mySqlConnection = GetMysqlConnection(productID);
                //startTime = new DateTime(2021, 12, 8, 8, 0, 0);
                //endTime = new DateTime(2021, 12, 8, 9, 0, 0);
                IEnumerable<YieldReportDetail> lstYieldReportDetail;
                string key = CacheKey.RepairYield + $"{productID}{startTime}{endTime}{viewerConfigID}{lineID}{reportType}{processNO}{reTestCount}{failCount}{wipCount}{conditionID}{stationID}{firstCount}";
                string keyMsg = key + "Msg";
                string msg = string.Empty;
                _memoryCache.TryGetValue(keyMsg, out msg);
                if (!_memoryCache.TryGetValue(key, out lstYieldReportDetail))
                {
                    lstYieldReportDetail = _yieldRepository.GetYieldDetailData(productID, viewerConfigID, startTime, endTime, lineID, reportType, processNO,
                    reTestCount, failCount, wipCount, conditionID, stationID, firstCount, mySqlConnection, ref msg);
                    _memoryCache.Set(key, lstYieldReportDetail, new TimeSpan(0, 0, 30));
                    _memoryCache.Set(keyMsg, msg, new TimeSpan(0, 0, 60));
                }
                return new ReturnInfoLayUI<YieldReportDetail>(msg, lstYieldReportDetail);
            }
            catch (Exception ex)
            {
                return new ReturnInfoLayUI<YieldReportDetail>(ex.ToString(), 1);
            }
        }

        [HttpGet]
        [Route("YieldReportDetailChart")]
        public ReturnInfoLayUI<YieldReportDetailChart> GetYieldReportDetailChartInfo(int productID, DateTime startTime, DateTime endTime, int viewerConfigID = 64
                , string lineID = "ALL", string reportType = "A", string processNO = "",
                int reTestCount = 0, int failCount = 0, int wipCount = 0, string conditionID = "", string stationID = "FM7010", int firstCount = 1, string failType = "")
        {
            try
            {
                MySqlConnection mySqlConnection = GetMysqlConnection(productID);
                //startTime = new DateTime(2021, 12, 8, 8, 0, 0);
                //endTime = new DateTime(2021, 12, 8, 9, 0, 0);
                IEnumerable<YieldReportDetailChart> lstYieldReportDetailChart;
                string key = CacheKey.RepairYield + $"{productID}{startTime}{endTime}{viewerConfigID}{lineID}{reportType}{processNO}{reTestCount}{failCount}{wipCount}{conditionID}{stationID}{firstCount}{failType}";
                string keyMsg = key + "Msg";
                string msg = string.Empty;
                _memoryCache.TryGetValue(keyMsg, out msg);
                if (!_memoryCache.TryGetValue(key, out lstYieldReportDetailChart))
                {
                    lstYieldReportDetailChart = _yieldRepository.GetYieldDetailChartData(productID, viewerConfigID, startTime, endTime, lineID, reportType, processNO,
                    reTestCount, failCount, wipCount, conditionID, stationID, firstCount, failType, mySqlConnection, ref msg);
                    _memoryCache.Set(key, lstYieldReportDetailChart, new TimeSpan(0, 0, 30));
                    _memoryCache.Set(keyMsg, msg, new TimeSpan(0, 0, 60));
                }
                int totalQty = lstYieldReportDetailChart.Sum(p => p.Qty);
                int cumulatedQty = 0;
                foreach (YieldReportDetailChart ychart in lstYieldReportDetailChart)
                {
                    cumulatedQty = cumulatedQty + ychart.Qty;
                    ychart.Individual = Math.Round((double.Parse(ychart.Qty.ToString()) / totalQty) * 100, 2, MidpointRounding.AwayFromZero).ToString();
                    ychart.Cumulated = Math.Round(((double)cumulatedQty / totalQty) * 100, 2, MidpointRounding.AwayFromZero).ToString();
                    ychart.Actual = Math.Round((double.Parse(ychart.Qty.ToString()) / ychart.OutputCount) * 100, 2, MidpointRounding.AwayFromZero).ToString();
                }
                return new ReturnInfoLayUI<YieldReportDetailChart>(msg, lstYieldReportDetailChart);
            }
            catch (Exception ex)
            {
                return new ReturnInfoLayUI<YieldReportDetailChart>(ex.ToString(), 1);
            }
        }
        #endregion

        #region Rework yield
        [HttpGet]
        [Route("YieldReportRework")]
        public ReturnInfoLayUI<YieldReport> GetYieldReportReworkInfo(int productID, DateTime startTime, DateTime endTime, int viewerConfigID
        , string lineID, string processNO, EnumList.EnvironmentList environment = EnumList.EnvironmentList.Release)
        {
            try
            {
                MySqlConnection mySqlConnection = GetMysqlConnection(productID, environment);
                IEnumerable<YieldReport> lstYieldReport;
                string key = CacheKey.ReworkYield + $"{productID}{startTime}{endTime}{viewerConfigID}{lineID}{processNO}";
                string keyMsg = key + "Msg";
                string msg = string.Empty;
                _memoryCache.TryGetValue(keyMsg, out msg);
                if (!_memoryCache.TryGetValue(key, out lstYieldReport))
                {
                    lstYieldReport = _yieldRepository.GetYieldReworkData(productID, viewerConfigID, startTime, endTime, lineID, processNO, mySqlConnection, ref msg);
                    _memoryCache.Set(key, lstYieldReport, new TimeSpan(0, 0, 30));
                    _memoryCache.Set(keyMsg, msg, new TimeSpan(0, 0, 60));
                }
                return new ReturnInfoLayUI<YieldReport>(msg, lstYieldReport);
            }
            catch (Exception ex)
            {
                return new ReturnInfoLayUI<YieldReport>(ex.ToString(), 1);
            }
        }

        [HttpGet]
        [Route("YieldReportReworkDetail")]
        public ReturnInfoLayUI<YieldReportDetail> GetYieldReportReworkDetailInfo(int productID, int viewerConfigID, DateTime startTime, DateTime endTime, string lineID,
         string processNO, int failCount, int wipCount, string conditionID, string stationID, int passCount, EnumList.EnvironmentList environment = EnumList.EnvironmentList.Release)
        {
            try
            {
                MySqlConnection mySqlConnection = GetMysqlConnection(productID, environment);
                IEnumerable<YieldReportDetail> lstYieldReportDetail;
                string key = CacheKey.ReworkYield + $"{productID}{startTime}{endTime}{viewerConfigID}{lineID}{processNO}{failCount}{wipCount}{conditionID}{stationID}";
                string keyMsg = key + "Msg";
                string msg = string.Empty;
                _memoryCache.TryGetValue(keyMsg, out msg);
                if (!_memoryCache.TryGetValue(key, out lstYieldReportDetail))
                {
                    lstYieldReportDetail = _yieldRepository.GetYieldReworkDetailData(productID, viewerConfigID, startTime, endTime, lineID,
                            processNO, failCount, wipCount, conditionID, stationID, passCount, mySqlConnection, ref msg);
                    _memoryCache.Set(key, lstYieldReportDetail, new TimeSpan(0, 0, 30));
                    _memoryCache.Set(keyMsg, msg, new TimeSpan(0, 0, 60));
                }
                return new ReturnInfoLayUI<YieldReportDetail>(msg, lstYieldReportDetail);
            }
            catch (Exception ex)
            {
                return new ReturnInfoLayUI<YieldReportDetail>(ex.ToString(), 1);
            }
        }

        [HttpGet]
        [Route("YieldReportReworkDetailChart")]
        public ReturnInfoLayUI<YieldReportDetailChart> GetYieldReportReworkDetailChartInfo(int productID, int viewerConfigID, DateTime startTime, DateTime endTime, string lineID,
         string processNO, int failCount, int wipCount, string conditionID, string stationID, int passCount, string failType, EnumList.EnvironmentList environment = EnumList.EnvironmentList.Release)
        {
            try
            {
                MySqlConnection mySqlConnection = GetMysqlConnection(productID, environment);
                IEnumerable<YieldReportDetailChart> lstYieldReportDetailChart;
                string key = CacheKey.ReworkYield + $"{productID}{startTime}{endTime}{viewerConfigID}{lineID}{processNO}{failCount}{wipCount}{conditionID}{stationID}{failType}";
                string keyMsg = key + "Msg";
                string msg = string.Empty;
                _memoryCache.TryGetValue(keyMsg, out msg);
                if (!_memoryCache.TryGetValue(key, out lstYieldReportDetailChart))
                {
                    lstYieldReportDetailChart = _yieldRepository.GetYieldReworkDetailChartData(productID, viewerConfigID, startTime, endTime, lineID,
                            processNO, failCount, wipCount, conditionID, stationID, passCount, failType, mySqlConnection, ref msg);
                    _memoryCache.Set(key, lstYieldReportDetailChart, new TimeSpan(0, 0, 30));
                    _memoryCache.Set(keyMsg, msg, new TimeSpan(0, 0, 60));
                }
                int totalQty = lstYieldReportDetailChart.Sum(p => p.Qty);
                int cumulatedQty = 0;
                foreach (YieldReportDetailChart ychart in lstYieldReportDetailChart)
                {
                    cumulatedQty = cumulatedQty + ychart.Qty;
                    ychart.Individual = Math.Round((double.Parse(ychart.Qty.ToString()) / totalQty) * 100, 2, MidpointRounding.AwayFromZero).ToString();
                    ychart.Cumulated = Math.Round(((double)cumulatedQty / totalQty) * 100, 2, MidpointRounding.AwayFromZero).ToString();
                    ychart.Actual = Math.Round((double.Parse(ychart.Qty.ToString()) / ychart.OutputCount) * 100, 2, MidpointRounding.AwayFromZero).ToString();
                }
                return new ReturnInfoLayUI<YieldReportDetailChart>(msg, lstYieldReportDetailChart);
            }
            catch (Exception ex)
            {
                return new ReturnInfoLayUI<YieldReportDetailChart>(ex.ToString(), 1);
            }
        }
        #endregion
    }
}