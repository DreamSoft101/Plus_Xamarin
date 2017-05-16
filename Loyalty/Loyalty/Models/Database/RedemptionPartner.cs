using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public partial class RedemptionPartner
    {
        [PrimaryKey]
        public int PartnerID { get; set; }
        public string PartnerName { get; set; }
        public string PartnerEmail { get; set; }
        public string PartnerURL { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ContactNo { get; set; }
        public string ContactName { get; set; }
        public string strPostCode { get; set; }
        public string strPassword { get; set; }
        public string strLoginId { get; set; }
        public Nullable<System.DateTime> dtPwdGenerate { get; set; }
        public Nullable<System.DateTime> dtPwdExpiry { get; set; }
        public string strActiveCode { get; set; }
        public Nullable<bool> intChgPwdNxtLogin { get; set; }
    }
}
