using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Rbac.SystemUser
{
    public class IdCardInfo
    {
        public string IdCard { get; set; }
        public string Openid { get; set; }
        public string RealName { get; set; }
        public string OldCard { get; set; }
        public string Phone { get; set; }
        public string  Sex { get; set; }
    }
}
