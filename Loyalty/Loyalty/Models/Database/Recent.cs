using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public class Recent
    {
        public static int intMerchant = 1;
        public static int intMerchantProduct = 2;
        public Recent()
        {

        }

        [PrimaryKey]
        public Guid ID { get; set; }

        public int intType { get; set; }

        public Guid IDObject { get; set; }

        public string strTime { get; set; }
    }
}
