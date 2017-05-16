using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class ValidateOTPResult : ResponseBase
    {
        public ValidateOTPResult()
        {

        }

        public ValidateOTPResult(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }
}