
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MBB_Registration : ResponseBase
    {
        public MBB_Registration()
        {

        }
        public MBB_Registration(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }
}
