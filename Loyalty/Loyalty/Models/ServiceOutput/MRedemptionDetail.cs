using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MRedemptionDetail
    {
        public string RedemptionProductId { get; set; }
        public string ProductName { get; set; }
        public string RedemptionProductDetailId { get; set; }
        public int intProductType { get; set; }
        public int intRedeemOption { get; set; }
        public int Status { get; set; }
        public int Quantity { get; set; }
        public decimal RedeemPoints { get; set; }
    }
}
