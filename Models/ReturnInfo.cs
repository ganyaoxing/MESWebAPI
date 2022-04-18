namespace MESWebAPI.Models
{
#nullable disable
    public class ReturnInfo<T>
    {
        public ReturnInfo()
        {
            Status = true;
        }
        public ReturnInfo(List<T> result)
        {
            Status = true;
            Result = result;
        }
        public ReturnInfo(IEnumerable<T> result)
        {
            Status = true;
            Result = result.ToList();
        }
        public string Message { get; set; }
        public bool Status { get; set; }
        public List<T> Result { get; set; }
    }
}