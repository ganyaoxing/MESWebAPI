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
using MySql.Data;

namespace MESWebAPI.Controllers
{
#nullable disable
    [Route("[controller]")]
    public class TestController : MESControllerBase
    {
        public TestController(ILogger<ReportController> logger, IConfiguration config, IMemoryCache iMemoryCache) : base(logger, config, iMemoryCache)
        {

        }

        [HttpGet]
        [Route("ReturnJson")]
        public async Task<string> RepairInfo_Test()
        {
            List<RepairUser> lstRu = new List<RepairUser>();
            await Task.Run(() =>
            {
                lstRu.Add(new RepairUser() { Sno = "1212" });
                lstRu.Add(new RepairUser() { Sno = "rrrrrr" });
            });
            return "{\"code\":0,\"msg\":\"\",\"count\":1000,\"data\":[{\"id\":10000,\"username\":\"user-0\",\"sex\":\"女\",\"city\":\"城市-0\",\"sign\":\"签名-0\",\"experience\":255,\"logins\":24,\"wealth\":82830700,\"classify\":\"作家\",\"score\":57},{\"id\":10001,\"username\":\"user-1\",\"sex\":\"男\",\"city\":\"城市-1\",\"sign\":\"签名-1\",\"experience\":884,\"logins\":58,\"wealth\":64928690,\"classify\":\"词人\",\"score\":27},{\"id\":10002,\"username\":\"user-2\",\"sex\":\"女\",\"city\":\"城市-2\",\"sign\":\"签名-2\",\"experience\":650,\"logins\":77,\"wealth\":6298078,\"classify\":\"酱油\",\"score\":31},{\"id\":10003,\"username\":\"user-3\",\"sex\":\"女\",\"city\":\"城市-3\",\"sign\":\"签名-3\",\"experience\":362,\"logins\":157,\"wealth\":37117017,\"classify\":\"诗人\",\"score\":68},{\"id\":10004,\"username\":\"user-4\",\"sex\":\"男\",\"city\":\"城市-4\",\"sign\":\"签名-4\",\"experience\":807,\"logins\":51,\"wealth\":76263262,\"classify\":\"作家\",\"score\":6},{\"id\":10005,\"username\":\"user-5\",\"sex\":\"女\",\"city\":\"城市-5\",\"sign\":\"签名-5\",\"experience\":173,\"logins\":68,\"wealth\":60344147,\"classify\":\"作家\",\"score\":87},{\"id\":10006,\"username\":\"user-6\",\"sex\":\"女\",\"city\":\"城市-6\",\"sign\":\"签名-6\",\"experience\":982,\"logins\":37,\"wealth\":57768166,\"classify\":\"作家\",\"score\":34},{\"id\":10007,\"username\":\"user-7\",\"sex\":\"男\",\"city\":\"城市-7\",\"sign\":\"签名-7\",\"experience\":727,\"logins\":150,\"wealth\":82030578,\"classify\":\"作家\",\"score\":28},{\"id\":10008,\"username\":\"user-8\",\"sex\":\"男\",\"city\":\"城市-8\",\"sign\":\"签名-8\",\"experience\":951,\"logins\":133,\"wealth\":16503371,\"classify\":\"词人\",\"score\":14},{\"id\":10009,\"username\":\"user-9\",\"sex\":\"女\",\"city\":\"城市-9\",\"sign\":\"签名-9\",\"experience\":484,\"logins\":25,\"wealth\":86801934,\"classify\":\"词人\",\"score\":75},{\"id\":10010,\"username\":\"user-10\",\"sex\":\"女\",\"city\":\"城市-10\",\"sign\":\"签名-10\",\"experience\":1016,\"logins\":182,\"wealth\":71294671,\"classify\":\"诗人\",\"score\":34},{\"id\":10011,\"username\":\"user-11\",\"sex\":\"女\",\"city\":\"城市-11\",\"sign\":\"签名-11\",\"experience\":492,\"logins\":107,\"wealth\":8062783,\"classify\":\"诗人\",\"score\":6},{\"id\":10012,\"username\":\"user-12\",\"sex\":\"女\",\"city\":\"城市-12\",\"sign\":\"签名-12\",\"experience\":106,\"logins\":176,\"wealth\":42622704,\"classify\":\"词人\",\"score\":54},{\"id\":10013,\"username\":\"user-13\",\"sex\":\"男\",\"city\":\"城市-13\",\"sign\":\"签名-13\",\"experience\":1047,\"logins\":94,\"wealth\":59508583,\"classify\":\"诗人\",\"score\":63},{\"id\":10014,\"username\":\"user-14\",\"sex\":\"男\",\"city\":\"城市-14\",\"sign\":\"签名-14\",\"experience\":873,\"logins\":116,\"wealth\":72549912,\"classify\":\"词人\",\"score\":8},{\"id\":10015,\"username\":\"user-15\",\"sex\":\"女\",\"city\":\"城市-15\",\"sign\":\"签名-15\",\"experience\":1068,\"logins\":27,\"wealth\":52737025,\"classify\":\"作家\",\"score\":28},{\"id\":10016,\"username\":\"user-16\",\"sex\":\"女\",\"city\":\"城市-16\",\"sign\":\"签名-16\",\"experience\":862,\"logins\":168,\"wealth\":37069775,\"classify\":\"酱油\",\"score\":86},{\"id\":10017,\"username\":\"user-17\",\"sex\":\"女\",\"city\":\"城市-17\",\"sign\":\"签名-17\",\"experience\":1060,\"logins\":187,\"wealth\":66099525,\"classify\":\"作家\",\"score\":69},{\"id\":10018,\"username\":\"user-18\",\"sex\":\"女\",\"city\":\"城市-18\",\"sign\":\"签名-18\",\"experience\":866,\"logins\":88,\"wealth\":81722326,\"classify\":\"词人\",\"score\":74},{\"id\":10019,\"username\":\"user-19\",\"sex\":\"女\",\"city\":\"城市-19\",\"sign\":\"签名-19\",\"experience\":682,\"logins\":106,\"wealth\":68647362,\"classify\":\"词人\",\"score\":51}]}";
        }
    }
}