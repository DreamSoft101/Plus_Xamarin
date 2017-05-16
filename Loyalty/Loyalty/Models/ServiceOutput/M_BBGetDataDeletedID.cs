using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class M_BBGetDataDeletedID
    {
        public M_BBGetDataDeletedID()
        {
            MemberTypeIDs = new List<Guid>();
            MerchantIDs = new List<Guid>();
            MerchantCategoryIDs = new List<Guid>();
            MerchantProductIDs = new List<Guid>();
            RedemptionProductIDs = new List<Guid>();
            RedemptionPartnerIDs = new List<Guid>();
            RedemptionProductDetailIDs = new List<Guid>();
        }
        public List<Guid> MemberTypeIDs { get; set; }
        public List<Guid> MerchantIDs { get; set; }
        public List<Guid> MerchantCategoryIDs { get; set; }
        public List<Guid> MerchantProductIDs { get; set; }
        public List<Guid> RedemptionProductIDs { get; set; }
        public List<Guid> RedemptionPartnerIDs { get; set; }
        public List<Guid> RedemptionProductDetailIDs { get; set; }
    }
}
