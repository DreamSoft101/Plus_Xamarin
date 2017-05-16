using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MGetPortalMenu : ResponseBase
    {
        public List<PortalMenuDetail> PortalMenuDetails;

        public MGetPortalMenu()
        {

        }
        public MGetPortalMenu(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }
}