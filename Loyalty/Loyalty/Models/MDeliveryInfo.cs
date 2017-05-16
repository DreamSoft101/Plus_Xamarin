using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models
{
    public class MDeliveryInfo
    {
        public string ContactName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string DeliverCity { get; set; }
        public Guid DeliverStateId { get; set; }
        public Guid DeliverCountryId { get; set; }
        public string PostalCode { get; set; }
    }
}
