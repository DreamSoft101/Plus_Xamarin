using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models
{
    public class MasterAccountInfo
    {
        public int RefType { get; set; }
        public string RefNo { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }

        public Guid MemberID { get; set; }

    }
}
