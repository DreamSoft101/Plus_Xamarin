using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblBrand
    {
        public TblBrand()
        {

        }
        [PrimaryKey]
        public string idBrand { get; set; }
        public int intBrandType { get; set; }
        public string strName { get; set; }
        public string strDescription { get; set; }
        public string strPicture { get; set; }
        public int intStatus { get; set; }
        public string idEntity { get; set; }
    }
}
