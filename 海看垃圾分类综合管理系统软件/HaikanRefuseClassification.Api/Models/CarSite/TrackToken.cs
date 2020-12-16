using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCampusManagementWatchDog.Model
{
    public class TrackToken
    {
        /// <summary>
        /// 
        /// </summary>
        public AdditionalInformation additionalInformation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string expiration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string expired { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int expiresIn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RefreshToken refreshToken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> scope { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tokenType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }

        public class AdditionalInformation
        {
        }

        public class RefreshToken
        {
            /// <summary>
            /// 
            /// </summary>
            public string expiration { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }
    }
}
