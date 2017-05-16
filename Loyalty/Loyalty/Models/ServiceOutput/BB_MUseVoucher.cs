using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class BB_MUseVoucher : ResponseBase
    {
        public BB_MUseVoucher()
        {

        }
        public BB_MUseVoucher(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }
}
