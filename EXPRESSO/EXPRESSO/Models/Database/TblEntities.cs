using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblEntities
    {
        public TblEntities()
        {

        }

        [PrimaryKey]
        public string idEntity { get; set; }

        public string strName { get; set; }
    }
}
