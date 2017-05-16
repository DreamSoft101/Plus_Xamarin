using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblNotification
    {
        public TblNotification()
        {

        }
        [PrimaryKey]
        public string idHighway { get; set; }
        public string strMessage { get; set; }
        public int intStatus { get; set; }
        public long dtReceived { get; set; }
        public string idEntity { get; set; }
    }
}
