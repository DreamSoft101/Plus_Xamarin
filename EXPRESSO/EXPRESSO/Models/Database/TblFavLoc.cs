using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblFavLoc
    {
        public TblFavLoc()
        {

        }
        [PrimaryKey, AutoIncrement]
        public int idMyFav { get; set; }
        public string idObject { get; set; }
        public int intObjectType { get; set; }
        public string strDescription { get; set; }
        public string idEntity { get; set; }

        public int intStatus;
    }
}
