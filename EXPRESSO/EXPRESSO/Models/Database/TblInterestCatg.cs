using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblInterestCatg
    {
        public TblInterestCatg()
        {
            
        }

        [PrimaryKey]
        public int idInterestCatg { get; set; }
        public string strInterestCatgName { get; set; }
        public string idEntity { get; set; }
    }
}
