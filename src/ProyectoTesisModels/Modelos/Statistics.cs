using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoTesisModels.Modelos
{
    public class Statistics
    {
        public long totalTweetsProfiles { get; set; }
        public long totalTweetsBase { get; set; }
        public long totalTweetsCleanProfiles { get; set; }
        public long totalTweetsCleanBase { get; set; }
        public string maxDateProfiles { get; set; }
        public string maxDateBase { get; set; }
    }
}
