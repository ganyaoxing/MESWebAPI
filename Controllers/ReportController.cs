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

using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc.Core;
using MESWebAPI.Models;

using System.Data.SqlClient;
using System.Data;
using Dapper;
//using Microsoft.Data.SqlClient;
namespace MESWebAPI.Controllers
{
#nullable disable
    [ApiController]
    //[Route("basis/[controller]")]
    [Route("[controller]")]
    public class ReportController : MESControllerBase
    {
        Random _random = new Random(Guid.NewGuid().GetHashCode());
        public ReportController(ILogger<ReportController> logger, IConfiguration config, IMemoryCache iMemoryCache) : base(logger, config, iMemoryCache)
        {

        }

        [HttpGet]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("RepairTest")]
        public async Task<List<RepairUser>> RepairInfo_Test()
        {
            List<RepairUser> lstRu = new List<RepairUser>();
            await Task.Run(() =>
            {
                lstRu.Add(new RepairUser() { Sno = "1212" });
                lstRu.Add(new RepairUser() { Sno = "rrrrrr" });
            });
            return lstRu;
        }

        [HttpGet]
        [Route("Repair")]
        public async Task<ReturnInfo_MES<RepairUser>> RepairInfo(int productID, byte status, DateTime startTime, DateTime endTime)
        {
            ReturnInfo_MES<RepairUser> returnInfo = new ReturnInfo_MES<RepairUser>();
            BUMasterRepository masterRepository = await GetRepository<BUMasterRepository>(productID);
            IEnumerable<RepairUser> ieRepair = masterRepository.GetRepairUserInfo(productID, status, startTime, endTime);
            List<RepairUser> lstRepairInfo = ieRepair.ToList();

            returnInfo.SetData(lstRepairInfo);
            return returnInfo;
        }

        [HttpGet]
        [Route("ViewerConfigInfo")]
        public async Task<ReturnInfo<ViewerConfigInfo>> GetViewerConfigInfo(int productID = 29325982)
        {
            BUBasisRepository basisRepository = await GetRepository<BUBasisRepository>(productID);
            byte enable = (byte)EnumList.EnableList.Enable;
            IEnumerable<ViewerConfigInfo> ieViewerConfigInfo = basisRepository.GetViewerConfig(productID, enable);
            return new ReturnInfo<ViewerConfigInfo>(ieViewerConfigInfo); ;
        }
        [HttpGet]
        [Route("EnableProductsByBUName")]
        public ReturnInfo<Product> GetEnableProductsByBUName(string buName)
        {
            IEnumerable<Product> lstProduct;
            string key = CacheKey.EnableProduct + buName.ToUpper();
            if (!_memoryCache.TryGetValue(key, out lstProduct))
            {
                lstProduct = _basisRepository.GetProductByBUName(buName, true);
                _memoryCache.Set(key, lstProduct, new TimeSpan(1, 0, 0));
            }
            return new ReturnInfo<Product>(lstProduct);
        }
        [HttpGet]
        [Route("EnableProductsByBUID")]
        public async Task<ReturnInfo<Product>> GetEnableProductsByBUID(int buID)
        {
            IEnumerable<Product> lstProduct;
            string key = CacheKey.EnableProduct + buID.ToString();
            if (!_memoryCache.TryGetValue(key, out lstProduct))
            {
                lstProduct = await _basisRepository.GetProductByBU(buID, true);
                _memoryCache.Set(key, lstProduct, new TimeSpan(0, 1, 0));
            }

            return new ReturnInfo<Product>(lstProduct);
        }
        [HttpGet]
        [Route("EnableProcessNOByProduct")]
        public async Task<ReturnInfo<ProcessNOInfo>> GetEnableProcessNOByProduct(int productID)
        {
            IEnumerable<ProcessNOInfo> lstProcessNO;
            string key = CacheKey.EnableProduct + productID.ToString();
            if (!_memoryCache.TryGetValue(key, out lstProcessNO))
            {
                lstProcessNO = await _basisRepository.GetProcessNOByProduct(productID, true);
                _memoryCache.Set(key, lstProcessNO, new TimeSpan(0, 1, 0));
            }
            return new ReturnInfo<ProcessNOInfo>(lstProcessNO);
        }

        [HttpGet]
        [Route("GetEnableLineByBU")]
        public async Task<ReturnInfo<PDLine>> GetEnableLineByBU(int buid)
        {
            IEnumerable<PDLine> lstLine;
            string key = CacheKey.EnableProduct + buid.ToString();
            if (!_memoryCache.TryGetValue(key, out lstLine))
            {
                lstLine = await _basisRepository.GetProductLineByBU(buid, true);
                _memoryCache.Set(key, lstLine, new TimeSpan(0, 1, 0));
            }
            return new ReturnInfo<PDLine>(lstLine);
        }

        [HttpGet]
        public string Get()
        {
            return $"This is MES1API ReportController with version： {_config["Version"]}";
        }

        [HttpPost("sendyield")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> SendYield([FromForm] string sno, [FromForm] string jsondata)
        {
            if (string.IsNullOrEmpty(sno) || string.IsNullOrEmpty(jsondata))
            {
                return BadRequest("无效的请求参数！");
            }
            _logger.LogInformation("Recieve: {sno}", sno);

            _logger.LogDebug(jsondata);
            SnoInfo snoInfo = JsonConvert.DeserializeObject<SnoInfo>(jsondata);
            if (!string.IsNullOrEmpty(snoInfo.Brokers) && !string.IsNullOrEmpty(snoInfo.Topics))
            {
                _logger.LogDebug(snoInfo.Brokers);
                _logger.LogDebug(snoInfo.Topics);
                int maxPartitionQty = snoInfo.Brokers.Split(',').Length;
                var config = new ProducerConfig { BootstrapServers = snoInfo.Brokers };
                using (var p = new ProducerBuilder<Null, string>(config).Build())
                {
                    try
                    {
                        string jData = JsonConvert.SerializeObject(new
                        {
                            Sno = snoInfo.Sno,
                            TransactionLogID = snoInfo.TransactionLogID,
                            ProcessLogID = snoInfo.ProcessLogID,
                            LineID = snoInfo.LineID,
                            TestResult = snoInfo.TestResult,
                            StationID = snoInfo.StationID,
                            NextStationID = snoInfo.NextStationID,
                            ProcessNO = snoInfo.ProcessNO,
                            FISMOID = snoInfo.FISMOID,
                            BuildID = snoInfo.BuildID,
                            ReWorkCount = snoInfo.ReWorkCount,
                            ComputerName = snoInfo.ComputerName,
                            ProductID = snoInfo.ProductID,
                            TestTime = snoInfo.TestTime,
                            TestDate = snoInfo.TestDate,
                            SendDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                        });

                        int partion = maxPartitionQty > 1 ? _random.Next(maxPartitionQty) : 0;

                        var dr = await p.ProduceAsync(new TopicPartition(snoInfo.Topics, new Partition(partion)), new Message<Null, string>
                        {
                            Value = jData
                        });
                        _logger.LogInformation("Delivery succeed: {sno}, {stationid}", sno, snoInfo.StationID);
                    }
                    catch (ProduceException<Null, string> e)
                    {
                        _logger.LogError("Delivery failed: {reason}", e.Error.Reason);
                    }
                }
            }

            return Ok();
        }

        [HttpGet]
        private Task<string> AsyncTest()
        {
            var task = Task.Run(() =>
            {
                return "Hello I am async test";
            });
            return task;
        }
    }
}