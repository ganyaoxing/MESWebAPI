namespace MESWebAPI.Models
{
    #nullable disable
    public class SnoInfo
    {
        public string Sno { get; set; }
        public int TransactionLogID { get; set; }
        public int ProcessLogID { get; set; }
        public string LineID { get; set; }
        public string TestResult { get; set; }
        public string StationID { get; set; }
        public string NextStationID { get; set; }
        public string ProcessNO { get; set; }
        public int FISMOID { get; set; }
        public string BuildID { get; set; }
        public int ReWorkCount { get; set; }
        public string ComputerName { get; set; }
        public int ProductID { get; set; }
        public int TestTime { get; set; }
        public int ProcessType { get; set; }
        public string Brokers { get; set; }
        public string Topics { get; set; }
        public string TestDate { get; set; }
    }
}