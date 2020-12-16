using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiKanTaskHousekeeperCore
{
    public class weightinfo
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
            /// 
            /// </summary>
            public string WeightTime { get; set; }
            /// <summary>
            /// 浙A00002
            /// </summary>
            public string CarNumber { get; set; }
            /// <summary>
            /// 车辆称重
            /// </summary>
            public string RecordType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string IsCheck { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Dustbin { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string AddTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string TYPE { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double Weight { get; set; }
        }
    }
}
