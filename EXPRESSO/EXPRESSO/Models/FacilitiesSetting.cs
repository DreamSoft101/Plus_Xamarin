using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class FacilitiesSetting
    {
        public string strName { get; set; }
        public int intType { get; set; } //1. Facility, 2. Nearby
        public string intID { get; set; }
        public string strURL { get; set; }
    }
}

