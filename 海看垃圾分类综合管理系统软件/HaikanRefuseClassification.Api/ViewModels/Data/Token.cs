using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Data
{
    public class Token
    {
        /// <summary>
        /// 
        /// </summary>
        public string Access_token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Expires_in { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Errmsg { get; set; }
    }
}
