using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblFacilityType
    {
        public TblFacilityType()
        {

        }

        [PrimaryKey]
        public int intFacilityType { get; set; }
        public string strName { get; set; }
        public string strPicture { get; set; }
        public int intBrandType { get; set; }
        public int intFeatured { get; set; }
        public int intSort { get; set; }
        public int intStatus { get; set; }

        [PrimaryKey]
        public int idEntity { get; set; }
    }
}
