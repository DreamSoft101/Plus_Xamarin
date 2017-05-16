using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblUpdateHistory
    {
        public TblUpdateHistory GetAwaiter()
        {
            return default(TblUpdateHistory);
        }

        public TblUpdateHistory()
        {

        }
        [PrimaryKey,AutoIncrement]
        public int idUpdateHistory { get; set; }
        public string idUpdate { get; set; }
        public string strTitle { get; set; }
        public int intUpdateType { get; set; }
        public int intResult { get; set; }
        public string dtUpdatedTime { get; set; }
        public string strVersion { get; set; }
        public string strDescription { get; set; }
        public string idEntity { get; set; }
    }
}
