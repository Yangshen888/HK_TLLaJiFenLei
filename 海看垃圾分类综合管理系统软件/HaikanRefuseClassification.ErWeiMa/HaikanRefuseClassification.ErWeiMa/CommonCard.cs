using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaikanDemo
{
    public class CommonCard
    {
        public class Data
        {
            /// <summary>
            /// 
            /// </summary>
            public string verifyResp { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
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

    }
}