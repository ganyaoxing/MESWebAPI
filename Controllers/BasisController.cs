
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
using Newtonsoft.Json;

namespace MESWebAPI.Controllers
{
#nullable disable
    [ApiController]
    //[Route("basis/[controller]")]
    [Route("[controller]")]
    public class BasisController : MESControllerBase
    {
        // protected readonly ILogger<BasisController> _logger;
        // protected readonly IConfiguration _config;
        // protected readonly IMemoryCache _memoryCache;
        // protected BasisRepository _basisRepository;

        public BasisController(ILogger<BasisController> logger, IConfiguration config, IMemoryCache iMemoryCache) : base(logger, config, iMemoryCache)
        {
            string dynamicConn = Environment.GetEnvironmentVariable("BasisConnection");
            if (!string.IsNullOrEmpty(dynamicConn))
            {
                _basisRepository = new BasisRepository(AESHelper.AesDecrypt(dynamicConn));
            }
            else
            {
                _basisRepository = new BasisRepository(_config["ConnectionStrings:BasisConnection"]);
            }
        }
        [HttpGet]
        [HttpPost]
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
    }
}