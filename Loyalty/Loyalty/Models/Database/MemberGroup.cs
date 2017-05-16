using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public class MemberGroup
    {
        public System.Guid MemberGroupID { get; set; }
        public System.Guid MemberGroupTypeID { get; set; }
        public string MemberGroup1 { get; set; }
        public string Address { get; set; }
        public Nullable<System.Guid> CountryID { get; set; }
        public Nullable<System.Guid> StateID { get; set; }
        public Nullable<System.Guid> CityID { get; set; }
        public string Postcode { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<byte> intCustomerType { get; set; }
        public Nullable<System.Guid> idMemberGroup { get; set; }
        public Nullable<System.Guid> idDocument { get; set; }
    }
}
