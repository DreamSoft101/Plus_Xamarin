using Loyalty.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MBB_GetData : ResponseBase
    {
        public MBB_GetData()
        {

        }
        public MBB_GetData(ResponseBase p)
        {
            this.CopyFromBase(p);
        }

        public DateTime LastGet { get; set; }
        public List<MemberType> MemberTypes { get; set; }

        public List<Merchant> Merchants { get; set; }

        public List<MerchantCategory> MerchantCategories { get; set; }

        public List<MerchantProduct> MerchantProducts { get; set; }

        public List<RedemptionCategory> RedemptionCategories { get; set; }

        public List<RedemptionProduct> RedemptionProducts { get; set; }

        public List<RedemptionProductDetail> RedemptionProductDetails { get; set; }

        public List<RedemptionPartner> RedemptionPartners { get; set; }

        public List<MerchantLocation> MerchantLocations { get; set; }

        public M_BBGetDataDeletedID DeletedID { get; set; }

        public List<MerchantProductMemberType> MerchantProductMemberTypes { get; set; }

        public List<MemberGroup> MemberGroups { get; set; }

        public List<MemberGroupDetail> MemberGroupDetails { get; set; }

        public List<State> States;

        public List<Country> Countries;
    }
}
