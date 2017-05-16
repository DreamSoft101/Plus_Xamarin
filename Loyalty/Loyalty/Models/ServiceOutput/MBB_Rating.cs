using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MBB_Rating : ResponseBase
    {
        public MBB_Rating()
        {

        }
        public MBB_Rating(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }
}
