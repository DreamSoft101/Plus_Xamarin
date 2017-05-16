using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public partial class RedemptionProductDetail
    {
        [PrimaryKey]
        public System.Guid RedemptionProductDetailID { get; set; }
        public System.Guid RedemptionProductID { get; set; }
        public System.Guid CurrencyID { get; set; }
        public int RedeemPoint { get; set; }
        public decimal RedeemAmount { get; set; }
        public decimal SubsidyAmount { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<decimal> CashPayment { get; set; }
        public Nullable<int> CashMode { get; set; }
        public Nullable<int> NumberMonths { get; set; }
        public Nullable<System.Guid> MemberTypeID { get; set; }
        public Nullable<decimal> Cash { get; set; }
        public Nullable<decimal> InstallmentAmount { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<int> Duration { get; set; }
        public Nullable<byte> intRedeemMode { get; set; }
        public Nullable<byte> decMonthlyPayment { get; set; }
        public Nullable<decimal> decMonths { get; set; }
        public Nullable<decimal> decInterest { get; set; }
        public string strPlanNumber { get; set; }
        public string strOldPrice { get; set; }

        //public virtual RedemptionProduct RedemptionProduct { get; set; }
    }
}
