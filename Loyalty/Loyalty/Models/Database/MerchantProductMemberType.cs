using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public class MerchantProductMemberType
    {
        [PrimaryKey]
        public System.Guid idMerchantProductMemberType { get; set; }
        public System.Guid idMerchantProduct { get; set; }
        public System.Guid idMemberType { get; set; }
        public Nullable<decimal> decOffer { get; set; }
        public Nullable<System.DateTime> dtValidFrom { get; set; }
        public Nullable<System.DateTime> dtValidTo { get; set; }
        public string strTerm { get; set; }
        public Nullable<System.DateTime> dtCreated { get; set; }
        public string strCreatedBy { get; set; }
        public Nullable<System.DateTime> dtModified { get; set; }
        public string strModifiedBy { get; set; }
    }
}
