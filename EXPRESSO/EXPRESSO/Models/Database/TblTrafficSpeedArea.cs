using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblTrafficSpeedArea
    {
        public TblTrafficSpeedArea()
        {

        }
        [PrimaryKey]
        public int idTrafficSpeedArea { get; set; }
        public string idHighway { get; set; }
        public int idHwayLocation { get; set; }
        public int intDirection { get; set; }
        public string strAreaName { get; set; }
        public string idEntity { get; set; }
    }
}
