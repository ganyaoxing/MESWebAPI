namespace MESWebAPI.Models
{
#nullable disable
    public class ProductDBConnection
    {
        public int ProductID { get; set; }
        public string BasisConnection { get; set; }
        public string RMABasisConnection { get; set; }
        public string MasterDB { get; set; }
        public string MeasureDB { get; set; }
        public string TransactionDB { get; set; }
        public string LogDB { get; set; }
        public string SNPoolDB { get; set; }
        public string PPSDB { get; set; }
        public string CPKDB { get; set; }
        public int BUID { get; set; }
    }
}