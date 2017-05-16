using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblVoucherType
    {
        public TblVoucherType()
        {

        }
        [PrimaryKey]
        public string idVoucherType { get; set; }
        public string strVoucherName { get; set; }
        public int intValidity { get; set; }
        public decimal decAmt { get; set; }
        public int intStatus { get; set; }
        public string idEntity { get; set; }
    }
}
