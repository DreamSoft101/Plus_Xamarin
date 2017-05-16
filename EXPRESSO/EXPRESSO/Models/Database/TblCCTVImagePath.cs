using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblCCTVImagePath
    {
        public TblCCTVImagePath()
        {

        }

        [PrimaryKey]
        public string idPatch { get; set; }

        public int idSource { get; set; }
        public string idEntity { get; set; }
        public int intType { get; set; }
        public string strURL { get; set; }
        public string strName { get; set; }
    }
}
