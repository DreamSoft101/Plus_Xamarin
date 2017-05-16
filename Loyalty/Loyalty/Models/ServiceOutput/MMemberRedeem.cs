using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MMemberRedeem : ResponseBase
    {

        public DateTime inTransactionDate { get; set; }
        public long inIDMasterAccount { get; set; }
        public decimal inTotalPointsRedeem { get; set; }
        public string RedeemCode { get; set; }
        public Guid RedemptionID { get; set; }
        public List<ErrorDetail> errors { get; set; }
        public MMemberRedeem()
        {
            errors = new List<ErrorDetail>();
        }
        public MMemberRedeem(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }

    public class ErrorDetail
    {
        public string Content { get; set; }
        public Guid ProductDetailID { get; set; }
        public ErrorDetail(string content, Guid? productDetailID)
        {
            this.Content = content;
            if (productDetailID != null)
            {
                this.ProductDetailID = (Guid)productDetailID;
            }

        }
    }
}
