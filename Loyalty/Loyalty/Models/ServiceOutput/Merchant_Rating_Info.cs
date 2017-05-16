using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class Merchant_Rating_Info
    {
        public System.Guid idMerchantProduct { get; set; }
        public Nullable<System.Guid> idMember { get; set; }
        public byte intRate { get; set; }
        public string strReview { get; set; }
        public System.DateTime dtCreated { get; set; }
        public string strCreatedBy { get; set; }
        public System.DateTime dtModified { get; set; }
        public string strModifiedBy { get; set; }
        public Nullable<double> idMasterAccount { get; set; }
        public double idPortalCredential { get; set; }
        public string ProductName { get; set; }
        public string UserName { get; set; }
    }
}
