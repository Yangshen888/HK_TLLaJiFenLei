using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.CarApp
{
    public class CheckWeighRecordModel
    {
        public List<List<string>> WeighData { get; set; }
        public Guid? Room { get; set; }
        public string CheckedUser { get; set; }
        public string Type { get; set; }
        public int Grade { get; set; }
    }
}
