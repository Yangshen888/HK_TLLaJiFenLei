using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Score
{
    public class ScoreSettingEditView
    {
        public Guid ScoreUuid { get; set; }
        public string ScoreName { get; set; }
        public int? Integral { get; set; }
        public string Addpeople { get; set; }
        public string AddTime { get; set; }
    }
}
