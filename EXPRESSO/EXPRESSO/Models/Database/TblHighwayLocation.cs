using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblHighwayLocation
    {
        public TblHighwayLocation()
        {

        }
        [PrimaryKey]
        public int idHwayLocation { get; set; }
        public string idHighway { get; set; }
        public decimal decLocation { get; set; }
        public double floLong { get; set; }
        public double floLat { get; set; }
        public string strRoute { get; set; }
        public string strRegion { get; set; }
        public int intDirection { get; set; }
        public string strSection { get; set; }
        public int intDefaultZoom { get; set; }
        public int intSort { get; set; }
    }
}
