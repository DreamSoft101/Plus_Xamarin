using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class RsaCCTV
    {
        public string idRSAcctv { get; set; }
        public  int idEntity { get; set; }
        public string idParent { get; set; }
        public int intParentType { get; set; }
        public string strTitle { get; set; }
        public string strURL { get; set; }
        public string strDescription { get; set; }
        public int intFrequency { get; set; }
        public int intVisible { get; set; }
        public int intStatus { get; set; }
        public string strCCTVImage { get; set; }
    }
}
