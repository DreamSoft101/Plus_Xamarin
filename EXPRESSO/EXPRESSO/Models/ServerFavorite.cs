using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class ServerFavorite
    {
        public static int DELETE = 2;
        public static int ADD_UPDATE = 1;

        public string idObject { get; set; }
        public int intType { get; set; }
        public int intStatus { get; set; }
    }
}
