using System;

namespace MESWebAPI.Models
{
    #nullable disable
    public class BU_NBA
    {
        public int BUID { get; set; }
        public string BUName { get; set; }
        public string Description { get; set; }
        public byte Enable { get; set; }
        public string ModifyUser { get; set; }
        public string AutoConfirm { get; set; }
        public string ReworkReason { get; set; }
        public List<Product> ProductList { get; set; }
    }
}
