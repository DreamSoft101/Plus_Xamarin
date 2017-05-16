using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class Merchant_Rating_Paging_Info
    {
        public DateTime dtCreated { get; set; }

        public byte intRate { get; set; }

        public string strReview { get; set; }
        public string UserName { get; set; }
    }
}
