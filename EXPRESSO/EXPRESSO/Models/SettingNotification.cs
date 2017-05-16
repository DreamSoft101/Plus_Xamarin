using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class SettingNotification
    {
        public string idParent { get; set; }
        public int intType { get; set;  } //1 LiveTraffic, 2 LiveFeed
        public DateTime dtTime { get; set; }

        public DateTime dtLastRun { get; set; }

        public long Tick { get; set; }

    }
}
