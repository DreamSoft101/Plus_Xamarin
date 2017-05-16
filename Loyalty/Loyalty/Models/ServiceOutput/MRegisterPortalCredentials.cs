using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MRegisterPortalCredentials : ResponseBase
    {
        public MRegisterPortalCredentials()
        {

        }

        public MRegisterPortalCredentials(ResponseBase p)
        {
            this.CopyFromBase(p);
        }


    }
}