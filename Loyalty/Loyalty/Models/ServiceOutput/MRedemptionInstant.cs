using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MRedemptionInstant : MRedemptionDetail
    {
        public long RedemptionDetailID { get; set; }
        public string RedeemAmount { get; set; }
        public string TransactionAmount { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }

    }
}
