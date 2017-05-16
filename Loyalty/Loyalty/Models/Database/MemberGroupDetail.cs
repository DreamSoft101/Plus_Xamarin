using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public class MemberGroupDetail
    {
        public Guid MemberGroupID { get; set; }
        public Guid MemberTypeID { get; set; }
        public System.Guid MemberGroupDetailID { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }

    }
}
