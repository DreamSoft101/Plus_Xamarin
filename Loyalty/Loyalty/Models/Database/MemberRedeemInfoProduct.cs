using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public class MemberRedeemInfoProduct
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Guid RedemptionProductId { get; set; }
        public Guid RedemptionProductDetailId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }

        public int Points { get; set; }
        public string strOldPrice { get; set; }

        public bool IsLock { get; set; }

        public int intType { get; set; }

        public string strURLImage { get; set; }
    }
}
