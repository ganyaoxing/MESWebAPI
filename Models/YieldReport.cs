namespace MESWebAPI.Models
{
#nullable disable
    public class YieldReport
    {
        public string ProcessNOLevel { get; set; }
        public string ProcessNO { get; set; }
        public string ModelID { get; set; }
        public string ConditionID { get; set; }
        public string ConditionName { get; set; }
        public int DefectShowType { get; set; }
        public DateTime EndTime { get; set; }
        public int FailCount { get; set; }
        private int _finalOutputCount { get; set; }
        public int FinalOutputCount
        {
            get
            {
                return _finalOutputCount == 0 ? OutputCount + VPYCount : _finalOutputCount;
            }
            set
            {
                _finalOutputCount = value;
            }
        }
        public int FirstCount { get; set; }
        public int FirstFailCount { get; set; }
        public decimal FirstYieldRate { get; set; }
        public string GUID { get; set; }
        public int InputCount { get; set; }
        public string LineID { get; set; }
        public int OutputCount { get; set; }
        public string ReportType { get; set; }
        public int ReTestCount { get; set; }
        public decimal RetestRate { get; set; }
        public DateTime StartTime { get; set; }
        public string StationID { get; set; }
        public string StationName { get; set; }
        public decimal TargetYieldRate { get; set; }
        public string TestDesk { get; set; }
        public int ViewerConfigID { get; set; }
        public decimal VPY { get; set; }
        public int VPYCount { get; set; }
        public int VPYCount2 { get; set; }
        public int WIP { get; set; }
        public int WIP1 { get; set; }
        public int WIP2 { get; set; }
        public int WIPCount { get; set; }
        public decimal YieldRate { get; set; }
    }
}