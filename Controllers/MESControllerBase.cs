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

namespace MESWebAPI.Controllers
{
#nullable disable
    [Route("[controller]")]
    public class MESControllerBase : ControllerBase
    {
        protected readonly IMemoryCache _memoryCache;
        protected readonly ILogger<MESControllerBase> _logger;
        protected readonly IConfiguration _config;
        protected BasisRepository _basisRepository;

        public MESControllerBase(ILogger<MESControllerBase> logger, IConfiguration config, IMemoryCache iMemoryCache)
        {
            _logger = logger;
            _config = config;
            _memoryCache = iMemoryCache;

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
        protected async Task<T> GetRepository<T>(int buid, int productid) where T : Repository, new()
        {
            Dictionary<string, string> dicDB = await GetProductConn(buid, productid);
            string conn;
            if (dicDB.TryGetValue(typeof(T).Name, out conn))
            {
                if (!string.IsNullOrEmpty(conn))
                {
                    T t = new T();
                    t.SetConnectionString(conn);
                    return t;
                }
            }
            return null;
        }
        [HttpGet]
        private async Task<Dictionary<string, string>> GetProductConn(int buid, int productid)
        {
            Dictionary<string, string> dicDBConn;
            string key = $"{CacheKey.ProductConnPrefix}-{buid.ToString()}-{productid.ToString()}";
            if (!_memoryCache.TryGetValue(key, out dicDBConn))
            {
                dicDBConn = new Dictionary<string, string>();
                dicDBConn.Add("BasisRepository", _basisRepository.ConnectionString);
                dicDBConn.Add("GlobalSNPoolRepository", _basisRepository.ConnectionString.Replace("FRAMEWORK_BASIS_V1", "FRAMEWORK_GLOBALSNPOOL_V1"));

                BUBasisDBConnection buBasisDBConn = await _basisRepository.GetBUBasisConnection(buid);
                if (buBasisDBConn != null)
                {
                    if (productid > 0)
                        dicDBConn.Add("BUBasisRepository", buBasisDBConn.BasisConnection ?? "");
                    else
                        dicDBConn.Add("BUBasisRepository", buBasisDBConn.RMABasisConnection ?? "");
                }
                else
                    dicDBConn.Add("BUBasisRepository", "");

                ProductDBConnection productDBConn = await _basisRepository.GetProductConnection(productid);
                if (productDBConn != null)
                {
                    dicDBConn.Add("BUCPKRepository", productDBConn.CPKDB ?? "");
                    dicDBConn.Add("BULogRepository", productDBConn.LogDB ?? "");
                    dicDBConn.Add("BUMasterRepository", productDBConn.MasterDB ?? "");
                    dicDBConn.Add("BUMeasureRepository", productDBConn.MeasureDB ?? "");
                    dicDBConn.Add("BUPPSRepository", productDBConn.PPSDB ?? "");
                    dicDBConn.Add("BUSNPoolRepository", productDBConn.SNPoolDB ?? "");
                    dicDBConn.Add("BUTransactionRepository", productDBConn.TransactionDB ?? "");
                }
                else
                {
                    dicDBConn.Add("BUCPKRepository", "");
                    dicDBConn.Add("BULogRepository", "");
                    dicDBConn.Add("BUMasterRepository", "");
                    dicDBConn.Add("BUMeasureRepository", ""); ;
                    dicDBConn.Add("BUPPSRepository", "");
                    dicDBConn.Add("BUSNPoolRepository", "");
                    dicDBConn.Add("BUTransactionRepository", "");
                }

                _memoryCache.Set(key, dicDBConn, new TimeSpan(12, 0, 0));
            }
            return dicDBConn;
        }

        [HttpGet]
        protected async Task<T> GetRepository<T>(int productid) where T : Repository, new()
        {
            Dictionary<string, string> dicDB = await GetProductConn(productid);
            string conn;
            if (dicDB.TryGetValue(typeof(T).Name, out conn))
            {
                if (!string.IsNullOrEmpty(conn))
                {
                    T t = new T();
                    t.SetConnectionString(conn);
                    return t;
                }
            }
            return null;
        }
        [HttpGet]
        private async Task<Dictionary<string, string>> GetProductConn(int productid)
        {
            Dictionary<string, string> dicDBConn;
            string key = $"{CacheKey.ProductConnPrefix}-{productid.ToString()}";
            if (!_memoryCache.TryGetValue(key, out dicDBConn))
            {
                dicDBConn = new Dictionary<string, string>();
                dicDBConn.Add("BasisRepository", _basisRepository.ConnectionString);
                dicDBConn.Add("GlobalSNPoolRepository", _basisRepository.ConnectionString.Replace("FRAMEWORK_BASIS_V1", "FRAMEWORK_GLOBALSNPOOL_V1"));

                ProductDBConnection productDBConn = await _basisRepository.GetProductConnection(productid);
                if (productDBConn != null)
                {
                    dicDBConn.Add("BUBasisRepository", productDBConn.BasisConnection ?? "");
                    dicDBConn.Add("BUCPKRepository", productDBConn.CPKDB ?? "");
                    dicDBConn.Add("BULogRepository", productDBConn.LogDB ?? "");
                    dicDBConn.Add("BUMasterRepository", productDBConn.MasterDB ?? "");
                    dicDBConn.Add("BUMeasureRepository", productDBConn.MeasureDB ?? "");
                    dicDBConn.Add("BUPPSRepository", productDBConn.PPSDB ?? "");
                    dicDBConn.Add("BUSNPoolRepository", productDBConn.SNPoolDB ?? "");
                    dicDBConn.Add("BUTransactionRepository", productDBConn.TransactionDB ?? "");
                }
                else
                {
                    dicDBConn.Add("BUBasisRepository", "");
                    dicDBConn.Add("BUCPKRepository", "");
                    dicDBConn.Add("BULogRepository", "");
                    dicDBConn.Add("BUMasterRepository", "");
                    dicDBConn.Add("BUMeasureRepository", ""); ;
                    dicDBConn.Add("BUPPSRepository", "");
                    dicDBConn.Add("BUSNPoolRepository", "");
                    dicDBConn.Add("BUTransactionRepository", "");
                }

                _memoryCache.Set(key, dicDBConn, new TimeSpan(12, 0, 0));
            }
            return dicDBConn;
        }
    }
}