using EXPRESSO.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class TollPlazaCCTV
    {
        public string idTollPlazaCctv { get; set; }
        public string idEntity { get; set; }
        public string idParent { get; set; }
        public string intParentType { get; set; }
        public string strTitle { get; set; }
        public string strURL { get; set; }
        public string strDescription { get; set; }
        public string intFrequency { get; set; }
        public string intVisible { get; set; }
        public string intStatus { get; set; }
        public string idHighway { get; set; }
        public string strCCTVImage { get; set; }

        public double decLat { get; set; }
        public double decLng { get; set; }
        //public TblTollPlaza mTollPlaza;
    }
}
