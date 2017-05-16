using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public  class MerchantProduct
    {
        [PrimaryKey]
        public System.Guid MerchantProductID { get; set; }
        public System.Guid MerchantID { get; set; }
        public System.Guid MerchantCategoryID { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public Nullable<byte> Badge { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool FeaturedProduct { get; set; }
        public Nullable<System.Guid> MainImageID { get; set; }
        public Nullable<System.Guid> ThumbnailImageID { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<System.Guid> DocumentID { get; set; }
        public string TermsAndConditions { get; set; }
        public string FileToUpload { get; set; }
        public Nullable<short> Sorting { get; set; }
     
        public string strLongDesc { get; set; }

        public double decRating { get; set; }
    }
}
