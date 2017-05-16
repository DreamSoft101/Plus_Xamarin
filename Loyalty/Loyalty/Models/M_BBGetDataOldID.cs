using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models
{
    public class M_BBGetDataOldID
    {
        public List<Guid> MemberTypeIDs { get; set; }
        public List<Guid> MerchantIDs { get; set; }
        public List<Guid> MerchantCategoryIDs { get; set; }
        public List<Guid> MerchantProductIDs { get; set; }
        public List<Guid> RedemptionProductIDs { get; set; }
        public List<Guid> RedemptionPartnerIDs { get; set; }
        public List<Guid> RedemptionProductDetailIDs { get; set; }
    }
}
