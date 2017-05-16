using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblFacilityImage
    {
        public TblFacilityImage()
        {

        }
        [PrimaryKey]
        public string idFacilityImg { get; set; }
        public string idFacilities { get; set; }
        public int intStatus { get; set; }
        public string strPicture { get; set; }
        public string strDescription { get; set; }
        public int intSort { get; set; }
        public string idEntity { get; set; }
    }
}
