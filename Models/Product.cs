using System;

namespace MESWebAPI.Models
{
    #nullable disable
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        /// <summary>
        /// FIS機種名稱
        /// </summary>
        public string FISProductName { get; set; }
        public int BUID { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// COM處理對應邏輯名稱
        /// </summary>
        public byte COMIdentityID { get; set; }
        /// <summary>
        /// 列印數量，1~255
        /// </summary>
        public byte PrintNumber { get; set; }
        public byte Enable { get; set; }
        public string ModifyUser { get; set; }
        /// <summary>
        /// 退整棧板時棧板號是否取消標記（0，不取消；1，取消。默認不取消）
        /// </summary>
        public byte PalletIDCancelFlag { get; set; }
        /// <summary>
        /// 棧板退中箱後再次刷入中箱是否沿用原棧板內中箱順序（0，不沿用；1，沿用。默認不沿用）
        /// </summary>
        public byte CartonIndexContinueFlag { get; set; }
        /// <summary>
        /// 新版本強制更新延遲天數（默認5天）
        /// </summary>
        public int ForceUpdateDay { get; set; }
        /// <summary>
        /// 設定此機種星期六,星期天數據是否可見（2，星期日,六數據可見；1，星期日,六數據不可見。默認2）
        /// </summary>
        public byte DataShowFlag { get; set; }
        /// <summary>
        /// 厂内序号打印是否采用可视化打印(0否;1是),默認否
        /// </summary>
        public byte ZebraVisualPrint { get; set; }
        /// <summary>
        /// BU名稱
        /// </summary>
        public string BUName { get; set; }
        /// <summary>
        /// 啟用狀態名稱
        /// </summary>
        public string EnableName { get; set; }
        /// <summary>
        /// COM識別名稱
        /// </summary>
        public string COMIdentityName { get; set; }
        public string ZebraVisualPrintName { get; set; }
    }
}
