using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblRoute
    {
        public TblRoute()
        {

        }
        [PrimaryKey]
        public string idRoute { get; set; }
        public string strRouteName { get; set; }
        public int intStatus { get; set; }
        public string idEntity { get; set; }
    }
}
