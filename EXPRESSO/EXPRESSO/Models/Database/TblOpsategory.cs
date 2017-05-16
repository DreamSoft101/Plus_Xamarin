using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblOpsategory
    {
        public TblOpsategory()
        {
            
        }
        [PrimaryKey]
        public int idOpsCategory { get; set; }
        public string strTitle { get; set; }
        public int intSource { get; set; }
        public long dtLastUpdate { get; set; }
        public string idEntity { get; set; }
    }
}
