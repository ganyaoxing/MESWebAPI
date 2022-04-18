namespace MESWebAPI.Models
{
#nullable disable
    public class YieldReportDetailChart
    {
        public string EngDesc { get; set; }
        public int Qty { get; set; }
        public string Individual { get; set; }
        public string Cumulated { get; set; }
        public string Actual { get; set; }
        public int OutputCount { get; set; }
    }
}