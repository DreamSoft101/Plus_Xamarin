using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MGetRedemptionDetails : ResponseBase
    {
        public long idMasterAccount { get; set; }
        public string RedeemCode { get; set; }
        public Guid inidRedemption { get; set; }
        public DateTime RedeemDate { get; set; }
        public string RedemptionNumber { get; set; }
        public decimal PointsRedeem { get; set; }
        public byte RedemptionStatus { get; set; }
        public List<MRedemptionDetail> RedeemProduct { get; set; }
        public MGetRedemptionDetails()
        {

        }
        public MGetRedemptionDetails(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }
}
