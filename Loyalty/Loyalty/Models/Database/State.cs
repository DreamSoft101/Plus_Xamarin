using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public class State
    {
        [PrimaryKey]
        public Guid RegionID { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
        public Guid RegionParentID { get; set; }
        public string Nationality { get; set; }
    }
}
