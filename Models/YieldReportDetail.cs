namespace MESWebAPI.Models
{
#nullable disable
    public class YieldReportDetail
    {
        public string ComputerName { get; set; }
        public string ConditionID { get; set; }
        public string CustomerSN { get; set; }
        public string DefectDesc { get; set; }
        public string DefectID { get; set; }
        public int DefectShowType { get; set; }
        public DateTime EndTime { get; set; }
        public int FailCount { get; set; }
        public string FailType { get; set; }
        public string FATPSN { get; set; }
        public int FirstCount { get; set; }
        public string GUID { get; set; }
        public string LineID { get; set; }
        public string ProcesslogID { get; set; }
        public string ReportType { get; set; }
        public int ReTestCount { get; set; }
        public string Sno { get; set; }
        public DateTime StartTime { get; set; }
        public string StationID { get; set; }
        public string TestDesk { get; set; }
        public string TestTime { get; set; }
        public int ViewerConfigID { get; set; }
        public int WIPCount { get; set; }
    }
}