using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public class MerchantLocation
    {
        public MerchantLocation()
        {

        }
        [PrimaryKey]
        public long idMerchantLocation { get; set; }
        public System.Guid MerchantId { get; set; }
        public string strLocationName { get; set; }
        public string strAddress { get; set; }
        public string strCountry { get; set; }
        public string strCity { get; set; }
        public string strPhone { get; set; }
        public string strEmail { get; set; }
        public string strLat { get; set; }
        public string strLng { get; set; }
        public System.Guid strCreateBy { get; set; }
        public System.DateTime dtCreateDate { get; set; }
        public System.Guid strLastUpdate { get; set; }
        public System.DateTime dtLastUpdate { get; set; }
    }
}
