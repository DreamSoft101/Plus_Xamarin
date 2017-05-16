using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class SentOTPResult : ResponseBase
    {
        public long idMasterAccount;
        public string SecurityKey;
        public Guid ID;
        public SentOTPResult()
        {

        }

        public SentOTPResult(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }
}