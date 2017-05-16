using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public partial class MemberType
    {
        public MemberType()
        {
          
        }

        public System.Guid MemberTypeID { get; set; }
        public string MemberType1 { get; set; }
        public string MemberTypeDesc { get; set; }
        public Nullable<int> Grade { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.Guid ModifiedBy { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public Nullable<decimal> AnnualFee { get; set; }
        public Nullable<System.Guid> PriceTypeID { get; set; }
        public byte ValidityPeriod { get; set; }
        public Nullable<long> idPointCurrency { get; set; }
        public string ReferenceCode { get; set; }

        //public virtual ICollection<MemberGroupDetail> MemberGroupDetails { get; set; }
        //public virtual ICollection<MemberTypeDetail> MemberTypeDetails { get; set; }
        //public virtual tblPointCurrencyValue tblPointCurrencyValue { get; set; }
        //public virtual ICollection<ProductReward> ProductRewards { get; set; }
        //public virtual ICollection<ProductPromotion> ProductPromotions { get; set; }
    }
}
