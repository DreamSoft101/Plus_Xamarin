using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class NearestLocation
    {
        public double floLong { get; set; }
        public double floLat { get; set; }
        public string strSection { get; set; }
        public double distance { get; set; }
    }
}
