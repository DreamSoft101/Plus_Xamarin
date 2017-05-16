using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MGetMenuPortal : ResponseBase
    {
        public List<MenuPortal> MenuPortals;

        public MGetMenuPortal()
        {

        }

        public MGetMenuPortal(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }
}
