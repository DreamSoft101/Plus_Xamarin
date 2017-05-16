using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MPaymentInformation
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string ApprovalNumber { get; set; }
    }
}
