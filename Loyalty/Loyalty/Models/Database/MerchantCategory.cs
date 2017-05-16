using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public partial class MerchantCategory
    {
        [PrimaryKey]
        public System.Guid MerchantCategoryID { get; set; }
        public Nullable<System.Guid> MerchantID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }

    }
}
