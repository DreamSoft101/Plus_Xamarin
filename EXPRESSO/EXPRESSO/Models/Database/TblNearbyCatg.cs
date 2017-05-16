using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblNearbyCatg
    {
        public TblNearbyCatg()
        {

        }
        [PrimaryKey]
        public string idNearbyCatg { get; set; }
        public string strNearbyCatgName { get; set; }
        public string strNearbyCatgImg { get; set; }
        public string strNearbyCatgImgSelect { get; set; }
        public int intSort { get; set; }
        public int intStatus { get; set; }
        public string idEntity { get; set; }
    }
}
