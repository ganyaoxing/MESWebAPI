using System;
using Newtonsoft.Json;
namespace MESWebAPI.Models;

#nullable disable
public class ApiResult
{
    [JsonProperty("code")]
    public int Code { get; set; }
    [JsonProperty("data")]
    public object Data { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
    [JsonProperty("count")]
    public int Count { get; set; }
    [JsonProperty("success")]
    public bool Success { get; set; }
    [JsonProperty("responsedatetime")]
    public string ResponseDatetime { get; set; }
    public ApiResult OK(object data, int count = 0)
    {
        Code = 0;
        Data = data;
        Message = "";
        Count = count;
        Success = true;
        ResponseDatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
        return this;
    }
    public ApiResult Fail(int errorCode, string errorMessage)
    {
        Code = errorCode;
        Data = null;
        Message = errorMessage;
        Count = 0;
        Success = false;
        ResponseDatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
        return this;
    }    
}