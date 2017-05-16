using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblTrafficSpeed
    {
        public TblTrafficSpeed()
        {

        }
        [PrimaryKey]
        public int idTrafficSpeed { get; set; }
        public int idTrafficSpeedArea { get; set; }
        public int intMarker { get; set; }
        public decimal decLng { get; set; }
        public decimal decLat { get; set; }
        public int intSpeed { get; set; }
        public int intRadius { get; set; }
        public string idEntity { get; set; }

    }
}
