using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class FavoriteLocation
    {
        public string idFavoriteLocation { get; set; }
        public string idHighway { get; set; }
        public string strFavoriteLocationName { get; set; }
        public List<string> detail { get; set; }
    }
}
