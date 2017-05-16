using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MRedemptionDetailPhysic : MRedemptionDetail
    {
        #region physical
        public decimal? RedeemAmount { get; set; }
        public string BatchNumber { get; set; }
        public string TrackingNumber { get; set; }
        public string ConsignmentNumber { get; set; }
        public DateTime? ResponseDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public DateTime? DeliverDate { get; set; }
        public MPaymentInformation PaymentInformation { get; set; }
        public Guid RedemptionDetailID { get; set; }
        #endregion
    }
}
