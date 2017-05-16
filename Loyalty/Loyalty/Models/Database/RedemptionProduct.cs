using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public partial class RedemptionProduct
    {
        public RedemptionProduct()
        {
            //this.MemberRedemptionDetails = new HashSet<MemberRedemptionDetail>();
           // this.RedemptionProductDetails = new HashSet<RedemptionProductDetail>();
        }
        [PrimaryKey]
        public System.Guid RedemptionProductID { get; set; }
        public System.Guid RedemptionCategoryID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<bool> TnGoVoucher { get; set; }
        public Nullable<decimal> VoucherReloadAmount { get; set; }
        public Nullable<bool> Available { get; set; }
        public Nullable<System.Guid> ThumbnailID { get; set; }
        public Nullable<System.Guid> ImageID { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public Nullable<int> ProductQuantity { get; set; }
        public Nullable<int> RedeemPoint { get; set; }
        public Nullable<decimal> RedeemAmount { get; set; }
        public Nullable<decimal> SubsidyAmount { get; set; }
        public Nullable<int> PartnerID { get; set; }
        public Nullable<int> StockInHand { get; set; }
        public Nullable<byte> intProductType { get; set; }
        public Nullable<int> intSorting { get; set; }
        public Nullable<System.DateTime> dtStartDate { get; set; }
        public Nullable<System.DateTime> dtEndDate { get; set; }
        public Nullable<decimal> decProductCost { get; set; }
        public Nullable<decimal> decDeliveryCost { get; set; }
        public Nullable<decimal> decTxnCost { get; set; }
        public Nullable<decimal> decInsuranceCost { get; set; }
        public string strWeight { get; set; }
        public string strWidth { get; set; }
        public string strHeight { get; set; }
        public string strLength { get; set; }
        public Nullable<decimal> decFeeValue { get; set; }
        public Nullable<decimal> decDeliverySLA { get; set; }
        public Nullable<int> intMaxFrequencyPerMonth { get; set; }
        public Nullable<int> intMaximumCashbackPerMonth { get; set; }
        public Nullable<decimal> decPercentPointBalanceCheck { get; set; }
        public Nullable<decimal> decCashValue { get; set; }
        public Nullable<long> idPointCurrency { get; set; }
        public Nullable<int> intPartnerPoint { get; set; }
        public Nullable<int> idFulFillmentHouse { get; set; }
        public Nullable<System.Guid> idAPIKey { get; set; }
        public Nullable<System.Guid> idPartnerPointExchange { get; set; }
        public bool bitHotDeal { get; set; }

        public Nullable<int> intAuthOnsite { get; set; }
        public string strAuthOnsiteCode { get; set; }
        public string strBarcodeMessage { get; set; }

        //public virtual ICollection<MemberRedemptionDetail> MemberRedemptionDetails { get; set; }
        //public virtual RedemptionCategory RedemptionCategory { get; set; }
        //public virtual ICollection<RedemptionProductDetail> RedemptionProductDetails { get; set; }
    }
}
