using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class RedemptionHistory
    {
        public Guid MemberRedemptionID { get; set; }
        public string strRedemptionNo { get; set; }
        public DateTime RedeemDate { get; set; }
        public byte RedemptionStatus { get; set; }
        public int Channel { get; set; }
        public int TotalPoint { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Address { get; set; }
    }
}
