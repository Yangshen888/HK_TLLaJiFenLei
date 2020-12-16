using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiKanTaskHousekeeperCore
{
    public class carinfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<BeanItem> bean { get; set; }

        public class BeanItem
        {
            /// <summary>
            /// 桐庐智慧环卫
            /// </summary>
            public string Company { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Photo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string GrabType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ADDTIME { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string BalanceID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Weight { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string CameraID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Brand { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string RegisterTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string WeightTime { get; set; }
            /// <summary>
            /// 浙A0Q805
            /// </summary>
            public string CarNum { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string AwardTime { get; set; }
            /// <summary>
            /// 垃圾车
            /// </summary>
            public string CarType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int IsDelete { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ID { get; set; }
        }
    }
}
