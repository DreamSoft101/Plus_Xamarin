using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblTollFare
    {
        public TblTollFare()
        {

        }

        [PrimaryKey]
        public string idTollFare { get; set; }
        public string idTollPlazaFrom { get; set; }
        public string idTollPlazaTo { get; set; }
        public decimal decTollAmt1 { get; set; }
        public decimal decTollAmt2 { get; set; }
        public decimal decTollAmt3 { get; set; }
        public decimal decTollAmt4 { get; set; }
        public decimal decTollAmt5 { get; set; }
        public int intStatus { get; set; }
        public string idEntity { get; set; }
    }
}
