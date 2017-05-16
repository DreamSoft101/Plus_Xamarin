using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MGetRedemptionHistory : ResponseBase
    {
        public List<RedemptionHistory> histories { get; set; }

    }
}