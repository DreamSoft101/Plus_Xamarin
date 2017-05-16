using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblLog
    {
        public TblLog()
        {

        }
        [PrimaryKey,AutoIncrement]
        public int idLog { get; set; }
        public string strDate { get; set; }
        public string strTag { get; set; }
        public string strContent { get; set; }
        public int intType { get; set;}
    }
}
