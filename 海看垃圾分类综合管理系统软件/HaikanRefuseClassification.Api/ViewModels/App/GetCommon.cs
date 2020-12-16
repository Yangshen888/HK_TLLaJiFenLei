using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.App
{
    public class GetCommon
    {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 操作成功！
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tradeCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
        public class Data
        {
            /// <summary>
            /// 
            /// </summary>
            public string healthQrCode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string qrCodeVaildDate { get; set; }
        }
    }
}
