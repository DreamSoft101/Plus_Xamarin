using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblHighway
    {
        public TblHighway()
        {

        }
        [PrimaryKey]
        public string idHighway { get; set; }
        public string strName { get; set; }
        public string strDescription { get; set; }
        public string strURLPath { get; set; }
        public double decStartLong { get; set; }
        public double decStartLat { get; set; }
        public double decEndLong { get; set; }
        public double decEndLat { get; set; }
        public int intStatus { get; set; }
        public string idEntity { get; set; }

    }
}
