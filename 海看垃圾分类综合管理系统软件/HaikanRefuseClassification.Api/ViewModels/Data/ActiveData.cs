using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Data
{
    public class ActiveData
    {
        /// <summary>
        /// 
        /// </summary>
        public System.Collections.Generic.List<Data.List> List { get; set; }
    }

    public class List
    {
        /// <summary>
        /// 日期，格式为 yyyymmdd
        /// </summary>
        public string Ref_date { get; set; }
        /// <summary>
        /// 打开次数
        /// </summary>
        public int Session_cnt { get; set; }
        /// <summary>
        /// 访问次数
        /// </summary>
        public int Visit_pv { get; set; }
        /// <summary>
        /// 访问人数
        /// </summary>
        public int Visit_uv { get; set; }
        /// <summary>
        /// 新用户数
        /// </summary>
        public int Visit_uv_new { get; set; }
        /// <summary>
        /// 人均停留时长 (浮点型，单位：秒)
        /// </summary>
        public double Stay_time_uv { get; set; }
        /// <summary>
        /// 次均停留时长 (浮点型，单位：秒)
        /// </summary>
        public double Stay_time_session { get; set; }
        /// <summary>
        /// 平均访问深度 (浮点型)
        /// </summary>
        public double Visit_depth { get; set; }
    }
}
