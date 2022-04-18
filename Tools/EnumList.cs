using MESWebAPI.CustomAttribute;

namespace MESWebAPI.Tools
{
#nullable disable
    public class EnumList
    {
        public enum RepairStatus
        {
            /// <summary>
            /// 預設值
            /// </summary>
            [EnumAttribute("預設值")]
            Default = 0,
            /// <summary>
            /// 未處理
            /// </summary>
            [EnumAttribute("未處理")]
            UnRepair = 1,
            /// <summary>
            /// 修復成功
            /// </summary>
            [EnumAttribute("修復成功")]
            Success = 2,
            /// <summary>
            /// 修復序號報廢
            /// </summary>
            [EnumAttribute("報廢")]
            Discard = 3,
            /// <summary>
            /// 未領板
            /// </summary>
            [EnumAttribute("未領板")]
            UnHandle = 4
        }
        public enum EnableList
        {
            [Enum("預設值")]
            Default = 0,
            [Enum("啟用")]
            Enable = 1,
            [Enum("不啟用")]
            NonEnable = 2,
        }
        public enum EnvironmentList
        {
            [Enum("預設值")]
            Default = 0,
            [Enum("正式")]
            Release = 1,
            [Enum("测试")]
            Testing = 2
            // [Enum("开发")]
            // Develop = 3
        }
    }
}