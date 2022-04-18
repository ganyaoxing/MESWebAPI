namespace MESWebAPI.Models
{
#nullable disable
    public class RepairUser
    {
        /// <summary>
        /// 廠內序號
        /// </summary>
        public string Sno { get; set; }
        /// <summary>
        /// 序号修复次数
        /// </summary>
        public int Rework { get; set; }
        /// <summary>
        /// 打不良是的Rework，方便之后查询不良明细
        /// </summary>
        public int RepairRework { get; set; }
        /// <summary>
        /// 刷入序号
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 修复状态：預設值=0；未處理=1；修復成功=2；報廢=3
        /// </summary>
        public byte Status { get; set; }
        /// <summary>
        /// 机种ID
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// 制程
        /// </summary>
        public string ProcessNO { get; set; }
        /// <summary>
        /// 制程阶：131,151，PA
        /// </summary>
        public string ProcessLevel { get; set; }
        /// <summary>
        /// 线别
        /// </summary>
        public string LineID { get; set; }
        /// <summary>
        /// 修复站别ID
        /// </summary>
        public string StationID { get; set; }
        /// <summary>
        /// 维修人员
        /// </summary>
        public string RepairUserID { get; set; }
        /// <summary>
        /// 修复成功时间
        /// </summary>
        public DateTime? RepairCDT { get; set; }

        #region 查询信息
        /// <summary>
        /// 修复站别名称
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// 机种名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 维修人员名称
        /// </summary>
        public string RepairUserName { get; set; }
        // <summary>
        /// 不良时间
        /// </summary>
        public DateTime? BindCDT { get; set; }
        /// <summary>
        /// 领板分配人员ID
        /// </summary>
        public string BindUserID { get; set; }
        /// <summary>
        /// 领板分配人员Name
        /// </summary>
        public string BindUserName { get; set; }
        /// <summary>
        /// 打不良人员ID
        /// </summary>
        public string DefectUserID { get; set; }
        /// <summary>
        /// 打不良人员Name
        /// </summary>
        public string DefectUserName { get; set; }
        // <summary>
        /// 不良时间
        /// </summary>
        public DateTime? DefectCDT { get; set; }

        #endregion
    }
}