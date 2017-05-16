using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    class MBB_UpdateProfile : ResponseBase
    {
        public MBB_UpdateProfile()
        {

        }
        public MBB_UpdateProfile(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }
}
