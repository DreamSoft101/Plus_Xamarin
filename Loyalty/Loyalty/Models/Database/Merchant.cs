using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public class Merchant
    {
        public Merchant()
        {
           
        }

        public System.Guid MerchantID { get; set; }
        public string MerchantName { get; set; }
        public string MerchantDesc { get; set; }
        public Nullable<System.Guid> LogoImageID { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<byte> Badge { get; set; }
        public bool intPublish { get; set; }

       
    }
}