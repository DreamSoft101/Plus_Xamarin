using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MIMX_GetPointBalance : ResponseBase
    {
        public string inCardNumber;
        public string inAccountReferenceNo;
        public int inAccountType;

        public Int64 idMasterAccount;
        public string strMasterAccountReferenceNo;
        public int intMasterAccountReferenceType;
        public Int64 intMasterAccountPointBalance;
        public MIMX_RewardDetail[] RewardDetails;

        public MIMX_GetPointBalance(ResponseBase p)
        {
            this.CopyFromBase(p);

        }

        public MIMX_GetPointBalance()
        {

        }
    }
}
