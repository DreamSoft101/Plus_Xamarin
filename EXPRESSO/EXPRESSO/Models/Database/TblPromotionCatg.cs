using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblPromotionCatg
    {
        public TblPromotionCatg()
        {

        }

        [PrimaryKey]
        public int idPromoCatg { get; set; }

        public string strPromoCatgName { get; set; }
        public string idEntity { get; set; }
    }
}
