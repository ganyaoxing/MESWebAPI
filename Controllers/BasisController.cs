
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

namespace MESWebAPI.Controllers
{
    #nullable disable
    [ApiController]
    //[Route("basis/[controller]")]
    [Route("[controller]")]
    public class BasisController: ControllerBase
    {
        protected readonly ILogger<BasisController> _logger;
        protected readonly IConfiguration _config;
        protected readonly IMemoryCache _memoryCache;
        protected BasisRepository _basisRepository;

        public BasisController(ILogger<BasisController> logger, IConfiguration config, IMemoryCache iMemoryCache)
        {
            _logger = logger;
            _config = config;
            _memoryCache =iMemoryCache;

            string dynamicConn = Environment.GetEnvironmentVariable("BasisConnection");
            if (!string.IsNullOrEmpty(dynamicConn))
            {
                _basisRepository =new BasisRepository(AESHelper.AesDecrypt(dynamicConn));
            }
            else
            {
                _basisRepository = new BasisRepository(_config["ConnectionStrings:BasisConnection"]);
            }
        }
        [HttpGet]
        public string Get()
        {
            return "122";
        }

        [Route("Test1/{userID}")]
        [HttpGet]
        public string Get11(string userID)
        {
            return userID;
        }
    }
}