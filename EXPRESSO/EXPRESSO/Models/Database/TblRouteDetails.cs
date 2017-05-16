using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblRouteDetails
    {
        public TblRouteDetails()
        {

        }
        [PrimaryKey, AutoIncrement]
        public int idDetail { get; set; }

        public string idRoute { get; set; }
        public int intSeq { get; set; }
        public int intType { get; set; }
        public string idRouteItem { get; set; }
        public double decLocation { get; set; }
        public string strExit { get; set; }
        public int intStatus { get; set; }
        public string idEntity { get; set; }
    }
}
