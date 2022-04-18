using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;

namespace MESWebAPI.CustomAttribute
{
#nullable disable
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false), ComVisible(true)]
    public class EnumAttribute : Attribute
    {
        private string description;
        /// <summary>
        /// 描述說明
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// EnumAttribute建構式，設定傳入描述說明
        /// </summary>
        /// <param name="description">string型態，存放說明的string物件</param>
        public EnumAttribute(string description)
       : base()
        {
            this.description = description;
        }
    }
}