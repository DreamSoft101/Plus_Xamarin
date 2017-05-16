using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblStretch
    {
        public TblStretch()
        {

        }

        public string idHighway { get; set; }
        public string strDirection { get; set; }
        public string strURLPath { get; set; }
        public decimal decStartLong { get; set; }
        public decimal decStartLat { get; set; }
        public decimal decEndLong { get; set; }
        public decimal decEndLat { get; set; }
        public int intStatus { get; set; }
        public string idEntity { get; set; }
    }
}
