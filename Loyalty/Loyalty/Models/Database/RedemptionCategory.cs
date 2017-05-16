using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public  class RedemptionCategory
    {
        public RedemptionCategory()
        {
           // this.RedemptionProducts = new HashSet<RedemptionProduct>();
        }
        [PrimaryKey]
        public System.Guid RedemptionCategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDesc { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<int> intSorting { get; set; }

        //public virtual ICollection<RedemptionProduct> RedemptionProducts { get; set; }
    }
}
