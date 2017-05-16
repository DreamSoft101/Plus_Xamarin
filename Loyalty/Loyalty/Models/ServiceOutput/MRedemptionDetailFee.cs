using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MRedemptionDetailFee : MRedemptionDetail
    {
        #region cash reward and fee knock-off
        public long RedemptionDetailID { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        #endregion

        #region fee knock-off
        public string ReferenceNo { get; set; }
        public string FeeItemName { get; set; }
        public decimal FeeAmount { get; set; }
        #endregion
    }
}
