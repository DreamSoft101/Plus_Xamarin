using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblPost
    {
        [PrimaryKey]
        public string idOpsComm { get; set; }
        public string strTitle { get; set; }
        public string strDescription { get; set; }
        public string strCustomerName { get; set; }
        public string strContactEmail { get; set; }
        public string idCategory { get; set; }
        public string strCreateBy { get; set; }
        public string dtCreateDate { get; set; }
        public string strLastUpdateBy { get; set; }
        public string dtLastUpdate { get; set; }
        public string decLatitude { get; set; }
        public string decLongitude { get; set; }
        public int intPosted { get; set; }
        public int intSource { get; set; }
        public int intStatus { get; set; }
        public List<Operations_Media> operations_media { get; set; }
        public string strAddress { get; set; }
        public string strAllowAccess { get; set; }
        public string strContactNo { get; set; }
        public string strUserPicture { get; set; }
        public string strCategory { get; set; }
    }
}
