namespace MESWebAPI.Models
{
#nullable disable
    public class ReturnInfo_MES<T>
    {
        public ReturnInfo_MES()
        {
            StatusCode = 200;
            Succeeded = true;
        }
        public void SetData(List<T> data)
        {
            Data = data;
            Timestamp = new TimeSpan(DateTime.Now.ToUniversalTime().Ticks - 621355968000000000).Ticks;
        }
        public int StatusCode { get; set; }
        public List<T> Data { get; private set; }
        public bool Succeeded { get; set; }
        public string Errors { get; set; }
        public string Extras { get; set; }
        public long Timestamp { get; private set; }
    }
}