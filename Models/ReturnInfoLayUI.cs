namespace MESWebAPI.Models
{
#nullable disable
    public class ReturnInfoLayUI<T>
    {
        public ReturnInfoLayUI(string msg, List<T> lstT)
        {
            Msg = msg;
            Count = lstT.Count();
            Data = lstT;
        }
        public ReturnInfoLayUI(string msg, IEnumerable<T> iemT)
        {
            Msg = msg;
            Count = iemT.Count();
            Data = iemT.ToList();
        }
        public ReturnInfoLayUI(string msg, int code)
        {
            Msg = msg;
            Code = code;
        }
        public int Code { get; set; }
        public string Msg { get; set; }
        public int Count { get; set; }
        public List<T> Data { get; set; }
    }
}