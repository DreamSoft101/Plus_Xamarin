using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblRSA
    {
        public TblRSA()
        {

        }
        [PrimaryKey]
        public string idRSA { get; set; }
        public string strType { get; set; }
        public string strName { get; set; }
        public string idHighway { get; set; }
        public string strSection { get; set; }
        public string strDirection { get; set; }
        public double decLocation { get; set; }
        public double decLong { get; set; }
        public double decLat { get; set; }
        public int intSort { get; set; }
        public string strSignatureName { get; set; }
        public string strPicture { get; set; }
        public int intStatus { get; set; }
        public string idEntity { get; set; }
    }
}
