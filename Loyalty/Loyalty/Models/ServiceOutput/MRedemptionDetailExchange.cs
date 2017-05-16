using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MRedemptionDetailExchange : MRedemptionDetail
    {
        #region partner exchange
        public long RedemptionDetailID { get; set; }
        public int idRedemptionPartnerExchange { get; set; }
        public Guid idPartnerExchange { get; set; }
        public string strPartnerName { get; set; }
        public string xmlInfo { get; set; }
        #endregion
    }
}
