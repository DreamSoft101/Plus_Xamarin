using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class FavoriteClass
    {
    }


    public class TollPlazaFavorite
    {
        public List<BaseItem> TollPlaza;
    }

    public class NearbyFavorite
    {
        public List<BaseItem> Nearby;


    }
    public class RSAFavorite
    {
        public List<BaseItem> RSA;
    }

    public class LiveFeedFavorite
    {
        public List<BaseItem> LiveFeed;
    }

    public class TrafficUpdateFavorite
    {
        public List<BaseItem> TrafficUpdate;
    }


    public class FavoriteHeader
    {
        public string strType { get; set; }
    }
}
