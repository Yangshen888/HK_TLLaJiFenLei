using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCampusManagementWatchDog.Model
{
    public class JWDBaidu
    {
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ResultItem> result { get; set; }

        public class ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public double x { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double y { get; set; }
        }
    }
}
