using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MIMX_AccountDetails
    {
        public string strCardNumber { get; set; }
        public string strMaskedPrincipalCardNumber { get; set; }
        public string strMaskedCardNumber { get; set; }
        public Guid idMember { get; set; }
        public Guid idMemberType { get; set; }
        public string strMobileNo { get; set; }
        public int intStatus { get; set; }
        public int intPointBalance { get; set; }
        public string strMemberTypeName { get; set; }
        public string dteExpiryDate { get; set; }
        public string strAttribute { get; set; }
        public int intAllowRedeem { get; set; }

        public MGetRewardAccountSummary Summary { get; set; }
    }
}
