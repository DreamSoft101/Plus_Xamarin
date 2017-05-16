using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblFacilities
    {
        public TblFacilities()
        {

        }
        [PrimaryKey]
        public string idFacilities { get; set; }
        public int intFacilityType { get; set; }
        public string strName { get; set; }
        public string idBrand { get; set; }
        public string strDescription { get; set; }
        public string strPicture { get; set; }
        public string strManager { get; set; }
        public string strContactNumber { get; set; }
        public int intParentType { get; set; }
        public string idParent { get; set; }
        public string idHighway { get; set; }
        public string strSection { get; set; }
        public string strDirection { get; set; }
        public double decLocation { get; set; }
        public double decLong { get; set; }
        public double decLat { get; set; }
        public int intSort { get; set; }
        public int intStatus { get; set; }
        public string idEntity { get; set; }
    }
}
