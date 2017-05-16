using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MRedemptionDetailVoucher : MRedemptionDetail
    {
        public long RedemptionDetailID { get; set; }
        public string C2DBarcodeMessage { get; set; }

        public string strVoucherNo { get; set; }
        public DateTime dtExpired { get; set; }
    }
}