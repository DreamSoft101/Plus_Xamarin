using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblTollFareOpenSystem
    {
        public TblTollFareOpenSystem()
        {

        }
        [PrimaryKey]
        public int idTollFareOS { get; set; }
        public string strName { get; set; }
        public string idHighway { get; set; }
        public string decLocation { get; set; }
        public decimal decTollAmt0 { get; set; }
        public decimal decTollAmt1 { get; set; }
        public decimal decTollAmt2 { get; set; }
        public decimal decTollAmt3 { get; set; }
        public decimal decTollAmt4 { get; set; }
        public decimal decTollAmt5 { get; set; }
        public decimal decTollAmt6 { get; set; }
        public decimal decTollAmt7 { get; set; }
        public string idEntity { get; set; }
    }
}
