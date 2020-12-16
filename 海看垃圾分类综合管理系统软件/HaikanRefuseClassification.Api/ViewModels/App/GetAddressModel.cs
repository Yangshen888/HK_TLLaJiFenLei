using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.App
{
    public class GetAddressModel
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Key { get; set; }
    }
}
