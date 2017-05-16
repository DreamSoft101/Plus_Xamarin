using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblOpsComm
    {
        public TblOpsComm()
        {

        }
        [PrimaryKey]
        public string idOpsComm { get; set; }

        public decimal decLat { get; set; }
        public decimal decLng { get; set; }
        public long dtCreateDate { get; set; }
        public long dtLastUpdate { get; set; }
        public int intPost { get; set; }
        public int intSource { get; set; }
        public int intStatus { get; set; }
        public string strAllowAccess { get; set; }
        public string idOpsCategory { get; set; }
        public string strTitle { get; set; }
        public string strDecscription { get; set; }
        public string strCustomerName { get; set; }
        public string strContractNo { get; set; }
        public string strContractEmail { get; set; }
        public string strCreateBy { get; set; }
        public string strLastUpdateBy { get; set; }
        public string strAvatar { get; set; }
        public string idEntity { get; set; }
    }
}
